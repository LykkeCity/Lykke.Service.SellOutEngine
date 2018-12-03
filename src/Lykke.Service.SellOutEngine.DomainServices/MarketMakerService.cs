using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Services;
using Lykke.Service.SellOutEngine.DomainServices.Traders;

namespace Lykke.Service.SellOutEngine.DomainServices
{
    [UsedImplicitly]
    public class MarketMakerService : IMarketMakerService
    {
        private readonly IInstrumentService _instrumentService;
        private readonly IQuoteService _quoteService;
        private readonly ILykkeExchangeService _lykkeExchangeService;
        private readonly IOrderBookService _orderBookService;
        private readonly IBalanceService _balanceService;
        private readonly IQuoteTimeoutSettingsService _quoteTimeoutSettingsService;
        private readonly IAssetPairsReadModelRepository _assetPairsReadModelRepository;
        private readonly ILog _log;

        public MarketMakerService(
            IInstrumentService instrumentService,
            IQuoteService quoteService,
            ILykkeExchangeService lykkeExchangeService,
            IOrderBookService orderBookService,
            IBalanceService balanceService,
            IQuoteTimeoutSettingsService quoteTimeoutSettingsService,
            IAssetPairsReadModelRepository assetPairsReadModelRepository,
            ILogFactory logFactory)
        {
            _instrumentService = instrumentService;
            _quoteService = quoteService;
            _lykkeExchangeService = lykkeExchangeService;
            _orderBookService = orderBookService;
            _balanceService = balanceService;
            _quoteTimeoutSettingsService = quoteTimeoutSettingsService;
            _assetPairsReadModelRepository = assetPairsReadModelRepository;
            _log = logFactory.CreateLog(this);
        }

        public async Task UpdateOrderBooksAsync()
        {
            IReadOnlyCollection<Instrument> instruments = await _instrumentService.GetAllAsync();

            IReadOnlyCollection<Instrument> activeInstruments = instruments
                .Where(o => o.IsApproved)
                .Where(o => o.Mode == InstrumentMode.Idle || o.Mode == InstrumentMode.Active)
                .ToArray();

            await Task.WhenAll(activeInstruments.Select(ProcessInstrumentAsync));
        }

        private async Task ProcessInstrumentAsync(Instrument instrument)
        {
            Quote quote = _quoteService.Get(instrument.QuoteSource, instrument.AssetPairId);

            if (quote == null)
            {
                _log.WarningWithDetails("No quote for instrument", new {instrument.AssetPairId});
                return;
            }

            AssetPair assetPair = _assetPairsReadModelRepository.TryGetIfEnabled(instrument.AssetPairId);

            if (assetPair == null)
            {
                _log.WarningWithDetails("Asset pair not found", new {instrument.AssetPairId});
                return;
            }

            Balance balance = await _balanceService.GetByAssetIdAsync(assetPair.BaseAssetId);

            IReadOnlyCollection<LimitOrder> limitOrders = EvenlyDistributedVolumeTrader.Calculate(quote, instrument,
                balance.Amount, assetPair.Accuracy, assetPair.InvertedAccuracy);

            await ValidateQuoteAsync(limitOrders, quote);

            ValidateMinVolume(limitOrders, assetPair.MinVolume);

            await _orderBookService.UpdateAsync(new OrderBook
            {
                AssetPairId = instrument.AssetPairId,
                Time = DateTime.UtcNow,
                LimitOrders = limitOrders
            });

            if (instrument.Mode != InstrumentMode.Active)
                SetError(limitOrders, LimitOrderError.Idle);

            limitOrders = limitOrders
                .Where(o => o.Error == LimitOrderError.None)
                .ToArray();

            try
            {
                await _lykkeExchangeService.ApplyAsync(instrument.AssetPairId, limitOrders);
            }
            catch (Exception exception)
            {
                _log.WarningWithDetails("An error occurred while processing limit orders", exception, limitOrders);

                SetError(limitOrders, LimitOrderError.Unknown, "ME error");
            }
        }

        private async Task ValidateQuoteAsync(IReadOnlyCollection<LimitOrder> limitOrders, Quote quote)
        {
            QuoteTimeoutSettings quoteTimeoutSettings = await _quoteTimeoutSettingsService.GetAsync();

            if (quoteTimeoutSettings.Enabled && DateTime.UtcNow - quote.Time > quoteTimeoutSettings.Value)
            {
                _log.WarningWithDetails("Quote timeout is expired", new
                {
                    quote,
                    Timeout = quoteTimeoutSettings.Value
                });

                SetError(limitOrders, LimitOrderError.InvalidQuote);
            }
        }

        private void ValidateMinVolume(IReadOnlyCollection<LimitOrder> limitOrders, decimal minVolume)
        {
            foreach (LimitOrder limitOrder in limitOrders.Where(o => o.Error == LimitOrderError.None))
            {
                if (limitOrder.Volume < minVolume)
                    limitOrder.Error = LimitOrderError.TooSmallVolume;
            }
        }

        private void SetError(IReadOnlyCollection<LimitOrder> limitOrders, LimitOrderError limitOrderError,
            string errorMessage = null)
        {
            foreach (LimitOrder limitOrder in limitOrders.Where(o => o.Error == LimitOrderError.None))
            {
                limitOrder.Error = limitOrderError;
                limitOrder.ErrorMessage = errorMessage;
            }
        }
    }
}
