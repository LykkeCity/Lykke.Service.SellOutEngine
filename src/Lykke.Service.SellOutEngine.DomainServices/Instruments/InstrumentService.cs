using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client.Models.v3;
using Lykke.Service.Assets.Client.ReadModels;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Exceptions;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Instruments
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILykkeExchangeService _lykkeExchangeService;
        private readonly IOrderBookService _orderBookService;
        private readonly IBalanceService _balanceService;
        private readonly IAssetsReadModelRepository _assetsReadModelRepository;
        private readonly IAssetPairsReadModelRepository _assetPairsReadModelRepository;
        private readonly ISettingsService _settingsService;
        private readonly InMemoryCache<Instrument> _cache;
        private readonly ILog _log;

        public InstrumentService(
            IInstrumentRepository instrumentRepository,
            ILykkeExchangeService lykkeExchangeService,
            IOrderBookService orderBookService,
            IBalanceService balanceService,
            IAssetsReadModelRepository assetsReadModelRepository,
            IAssetPairsReadModelRepository assetPairsReadModelRepository,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _instrumentRepository = instrumentRepository;
            _lykkeExchangeService = lykkeExchangeService;
            _orderBookService = orderBookService;
            _balanceService = balanceService;
            _assetsReadModelRepository = assetsReadModelRepository;
            _assetPairsReadModelRepository = assetPairsReadModelRepository;
            _settingsService = settingsService;
            _cache = new InMemoryCache<Instrument>(instrument => instrument.AssetPairId, false);
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<Instrument>> GetAllAsync()
        {
            IReadOnlyCollection<Instrument> instruments = _cache.GetAll();

            if (instruments == null)
            {
                instruments = await _instrumentRepository.GetAllAsync();

                _cache.Initialize(instruments);
            }

            return instruments;
        }

        public async Task<Instrument> GetByAssetPairIdAsync(string assetPairId)
        {
            IReadOnlyCollection<Instrument> instruments = await GetAllAsync();

            Instrument instrument = instruments.FirstOrDefault(o => o.AssetPairId == assetPairId);

            if (instrument == null)
                throw new EntityNotFoundException();

            return instrument;
        }

        public async Task AddAsync(Instrument instrument, string userId)
        {
            instrument.Approve();
            
            await _instrumentRepository.InsertAsync(instrument);

            _cache.Set(instrument);

            _log.InfoWithDetails("Instrument was added", new {instrument, userId});
        }

        public async Task CreateMissedAsync(string userId)
        {
            IReadOnlyCollection<string> sources = await _settingsService.GetQuoteSourcesAsync();
            
            IReadOnlyCollection<Balance> balances = await _balanceService.GetAsync();

            foreach (Balance balance in balances)
            {
                if(balance.Amount <= 0)
                    continue;
                
                Asset asset = _assetsReadModelRepository.TryGetIfEnabled(balance.AssetId);
                
                if(asset == null)
                    continue;

                AssetPair assetPair = _assetPairsReadModelRepository.GetAllEnabled()
                    .FirstOrDefault(o => o.BaseAssetId == asset.Id && o.QuotingAssetId == "USD");
                
                if(assetPair == null)
                    continue;
                
                IReadOnlyCollection<Instrument> instruments = await GetAllAsync();

                Instrument instrument = instruments.FirstOrDefault(o => o.AssetPairId == assetPair.Id);
                
                if(instrument != null)
                    continue;
                
                instrument = new Instrument
                {
                    AssetPairId = assetPair.Id,
                    QuoteSource = sources.FirstOrDefault(),
                    Markup = 0,
                    Levels = 1,
                    MinSpread = .2m,
                    MaxSpread = .8m,
                    Mode = InstrumentMode.Disabled,
                    IsApproved = false
                };
                
                await _instrumentRepository.InsertAsync(instrument);

                _cache.Set(instrument);
            }
            
            _log.InfoWithDetails("Missed instruments created", new {userId});
        }

        public async Task UpdateAsync(Instrument instrument, string userId)
        {
            Instrument currentInstrument = await GetByAssetPairIdAsync(instrument.AssetPairId);

            InstrumentMode currentInstrumentMode = currentInstrument.Mode;

            currentInstrument.Update(instrument);
            currentInstrument.Approve();
            
            await _instrumentRepository.UpdateAsync(currentInstrument);

            _cache.Set(currentInstrument);

            if (instrument.Mode == InstrumentMode.Disabled && currentInstrumentMode != InstrumentMode.Disabled)
            {
                try
                {
                    await _lykkeExchangeService.CancelAsync(instrument.AssetPairId);
                }
                catch (Exception exception)
                {
                    _log.WarningWithDetails("An error occurred while cancelling limit orders", exception,
                        new {currentInstrument, userId});
                }

                await _orderBookService.RemoveAsync(instrument.AssetPairId);
            }

            _log.InfoWithDetails("Instrument was updated", new {currentInstrument, userId});
        }

        public async Task DeleteAsync(string assetPairId, string userId)
        {
            Instrument instrument = await GetByAssetPairIdAsync(assetPairId);

            if (instrument.Mode == InstrumentMode.Active)
                throw new InvalidOperationException("Can not delete active instrument");

            await _instrumentRepository.DeleteAsync(assetPairId);

            _cache.Remove(assetPairId);

            await _orderBookService.RemoveAsync(instrument.AssetPairId);

            _log.InfoWithDetails("Instrument was deleted",  new {instrument, userId});
        }
    }
}
