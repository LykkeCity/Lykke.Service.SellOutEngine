using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.SellOutEngine.DomainServices.Timers;
using Lykke.Service.SellOutEngine.Rabbit.Subscribers;

namespace Lykke.Service.SellOutEngine.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly BalancesTimer _balancesTimer;
        private readonly MarketMakerTimer _marketMakerTimer;
        private readonly LykkeTradeSubscriber _lykkeTradeSubscriber;
        private readonly QuoteSubscriber[] _quoteSubscribers;

        public StartupManager(
            BalancesTimer balancesTimer,
            MarketMakerTimer marketMakerTimer,
            LykkeTradeSubscriber lykkeTradeSubscriber,
            QuoteSubscriber[] quoteSubscribers)
        {
            _balancesTimer = balancesTimer;
            _marketMakerTimer = marketMakerTimer;
            _lykkeTradeSubscriber = lykkeTradeSubscriber;
            _quoteSubscribers = quoteSubscribers;
        }

        public Task StartAsync()
        {
            _balancesTimer.Start();

            _lykkeTradeSubscriber.Start();

            foreach (QuoteSubscriber quoteSubscriber in _quoteSubscribers)
                quoteSubscriber.Start();

            _marketMakerTimer.Start();

            return Task.CompletedTask;
        }
    }
}
