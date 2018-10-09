using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.MatchingEngine.Connector.Models.Common;
using Lykke.MatchingEngine.Connector.Models.RabbitMq;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Services;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers;

namespace Lykke.Service.SellOutEngine.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class LykkeTradeSubscriber : IDisposable
    {
        private readonly SubscriberSettings _settings;
        private readonly ISettingsService _settingsService;
        private readonly ITradeService _tradeService;
        private readonly ISummaryReportService _summaryReportService;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqSubscriber<LimitOrders> _subscriber;

        public LykkeTradeSubscriber(
            SubscriberSettings settings,
            ISettingsService settingsService,
            ITradeService tradeService,
            ISummaryReportService summaryReportService,
            ILogFactory logFactory)
        {
            _settings = settings;
            _settingsService = settingsService;
            _tradeService = tradeService;
            _summaryReportService = summaryReportService;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue)
                .MakeDurable();

            settings.DeadLetterExchangeName = null;

            _subscriber = new RabbitMqSubscriber<LimitOrders>(_logFactory, settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<LimitOrders>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        private async Task ProcessMessageAsync(LimitOrders limitOrders)
        {
            try
            {
                if (limitOrders.Orders == null || limitOrders.Orders.Count == 0)
                    return;

                string walletId = await _settingsService.GetWalletIdAsync();

                if (string.IsNullOrEmpty(walletId))
                    return;

                IEnumerable<LimitOrderWithTrades> clientLimitOrders = limitOrders.Orders
                    .Where(o => o.Order?.ClientId == walletId)
                    .Where(o => o.Trades?.Count > 0);

                IReadOnlyCollection<Trade> trades = GetTrades(clientLimitOrders);

                if (trades.Any())
                {
                    foreach (var trade in trades)
                    {
                        await _tradeService.RegisterAsync(trade);
                        await _summaryReportService.RegisterTradeAsync(trade);
                    }

                    _log.InfoWithDetails("Traders were handled", clientLimitOrders);
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing trades", limitOrders);
            }
        }

        private static IReadOnlyCollection<Trade> GetTrades(IEnumerable<LimitOrderWithTrades> limitOrders)
        {
            var executionReports = new List<Trade>();

            foreach (LimitOrderWithTrades limitOrderModel in limitOrders)
            {
                if (limitOrderModel.Order.Status == OrderStatus.Matched ||
                    limitOrderModel.Order.Status == OrderStatus.Processing ||
                    limitOrderModel.Order.Status == OrderStatus.Cancelled)
                {
                    IReadOnlyList<Trade> orderExecutionReports =
                        GetTrades(limitOrderModel.Order, limitOrderModel.Trades);

                    executionReports.AddRange(orderExecutionReports);
                }
            }

            return executionReports;
        }

        private static IReadOnlyList<Trade> GetTrades(
            MatchingEngine.Connector.Models.RabbitMq.LimitOrder limitOrder,
            IReadOnlyList<LimitTradeInfo> trades)
        {
            var reports = new List<Trade>();

            for (int i = 0; i < trades.Count; i++)
            {
                LimitTradeInfo trade = trades[i];

                TradeType tradeType = limitOrder.Volume < 0
                    ? TradeType.Sell
                    : TradeType.Buy;

                reports.Add(new Trade
                {
                    Id = trade.TradeId,
                    AssetPairId = limitOrder.AssetPairId,
                    ExchangeOrderId = limitOrder.Id,
                    Type = tradeType,
                    Time = trade.Timestamp,
                    Price = (decimal) trade.Price,
                    Volume = tradeType == TradeType.Buy
                        ? (decimal) trade.OppositeVolume
                        : (decimal) trade.Volume,
                    OppositeClientId = trade.OppositeClientId,
                    OppositeLimitOrderId = trade.OppositeOrderId,
                    OppositeVolume = tradeType == TradeType.Buy
                        ? (decimal) trade.Volume
                        : (decimal) trade.OppositeVolume,
                    RemainingVolume = (decimal) limitOrder.RemainingVolume
                });
            }

            return reports;
        }
    }
}
