using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.MatchingEngine.Connector.Models.Events;
using Lykke.MatchingEngine.Connector.Models.Events.Common;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Deduplication;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Services;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers;
using Trade = Lykke.Service.SellOutEngine.Domain.Trade;

namespace Lykke.Service.SellOutEngine.Rabbit.Subscribers
{
    [UsedImplicitly]
    public class LykkeTradeSubscriber : IDisposable
    {
        private readonly SubscriberSettings _settings;
        private readonly ISettingsService _settingsService;
        private readonly ITradeService _tradeService;
        private readonly ISummaryReportService _summaryReportService;
        private readonly IDeduplicator _deduplicator;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private IStopable _subscriber;

        public LykkeTradeSubscriber(
            SubscriberSettings settings,
            ISettingsService settingsService,
            ITradeService tradeService,
            ISummaryReportService summaryReportService,
            IDeduplicator deduplicator,
            ILogFactory logFactory)
        {
            _settings = settings;
            _settingsService = settingsService;
            _tradeService = tradeService;
            _summaryReportService = summaryReportService;
            _deduplicator = deduplicator;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForSubscriber(_settings.ConnectionString, _settings.Exchange, _settings.Queue)
                .UseRoutingKey(((int) MessageType.Order).ToString())
                .MakeDurable();

            _subscriber = new RabbitMqSubscriber<ExecutionEvent>(
                    _logFactory, settings, new ResilientErrorHandlingStrategy(_logFactory, settings,
                        TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_logFactory, settings)))
                .SetMessageDeserializer(new ProtobufMessageDeserializer<ExecutionEvent>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetAlternativeExchange(_settings.AlternateConnectionString)
                .SetDeduplicator(_deduplicator)
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

        private async Task ProcessMessageAsync(ExecutionEvent message)
        {
            if (message.Header.MessageType != MessageType.Order)
                return;

            try
            {
                string walletId = await _settingsService.GetWalletIdAsync();

                Order[] orders = message.Orders
                    .Where(o => o.WalletId == walletId)
                    .Where(o => o.Side != OrderSide.UnknownOrderSide)
                    .Where(o => o.Trades?.Count > 0)
                    .ToArray();

                var trades = new List<Trade>();

                foreach (Order order in orders)
                {
                    if (order.Status == OrderStatus.Matched ||
                        order.Status == OrderStatus.PartiallyMatched ||
                        order.Status == OrderStatus.Cancelled)
                    {
                        IReadOnlyList<Trade> orderExecutionReports = GetTrades(order);

                        trades.AddRange(orderExecutionReports);
                    }
                }

                if (trades.Any())
                {
                    foreach (var trade in trades)
                    {
                        await _tradeService.RegisterAsync(trade);
                        await _summaryReportService.RegisterTradeAsync(trade);
                    }

                    _log.InfoWithDetails("Traders were handled", trades);
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception, "An error occurred during processing trades", message);
            }
        }

        private static IReadOnlyList<Trade> GetTrades(Order order)
        {
            var reports = new List<Trade>();

            for (int i = 0; i < order.Trades.Count; i++)
            {
                Lykke.MatchingEngine.Connector.Models.Events.Trade trade = order.Trades[i];

                TradeType tradeType = order.Side == OrderSide.Sell
                    ? TradeType.Sell
                    : TradeType.Buy;

                reports.Add(new Trade
                {
                    Id = trade.TradeId,
                    AssetPairId = order.AssetPairId,
                    ExchangeOrderId = order.Id,
                    LimitOrderId = order.ExternalId,
                    Type = tradeType,
                    Time = trade.Timestamp,
                    Price = decimal.Parse(trade.Price),
                    Volume = Math.Abs(decimal.Parse(trade.BaseVolume)),
                    OppositeClientId = trade.OppositeWalletId,
                    OppositeLimitOrderId = trade.OppositeOrderId,
                    OppositeVolume = Math.Abs(decimal.Parse(trade.QuotingVolume))
                });
            }

            return reports;
        }
    }
}
