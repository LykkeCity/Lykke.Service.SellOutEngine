using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Timers
{
    [UsedImplicitly]
    public class MarketMakerTimer : Timer
    {
        private readonly IMarketMakerService _marketMakerService;
        private readonly ITimersSettingsService _timersSettingsService;

        public MarketMakerTimer(
            IMarketMakerService marketMakerService,
            ITimersSettingsService timersSettingsService,
            ILogFactory logFactory)
        {
            _marketMakerService = marketMakerService;
            _timersSettingsService = timersSettingsService;
            Log = logFactory.CreateLog(this);
        }

        protected override Task OnExecuteAsync(CancellationToken cancellation)
        {
            return _marketMakerService.UpdateOrderBooksAsync();
        }

        protected override async Task<TimeSpan> GetDelayAsync()
        {
            TimersSettings timersSettings = await _timersSettingsService.GetAsync();

            return timersSettings.MarketMaker;
        }
    }
}
