using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Settings
{
    [UsedImplicitly]
    public class TimersSettingsService : ITimersSettingsService
    {
        private const string CacheKey = "key";

        private readonly ITimersSettingsRepository _timersSettingsRepository;
        private readonly InMemoryCache<TimersSettings> _cache;

        public TimersSettingsService(ITimersSettingsRepository timersSettingsRepository)
        {
            _timersSettingsRepository = timersSettingsRepository;
            _cache = new InMemoryCache<TimersSettings>(settings => CacheKey, true);
        }

        public async Task<TimersSettings> GetAsync()
        {
            TimersSettings timersSettings = _cache.Get(CacheKey);

            if (timersSettings == null)
            {
                timersSettings = await _timersSettingsRepository.GetAsync();

                if (timersSettings == null)
                {
                    timersSettings = new TimersSettings
                    {
                        MarketMaker = TimeSpan.FromSeconds(5),
                        Balances = TimeSpan.FromSeconds(1)
                    };
                }

                _cache.Initialize(new[] {timersSettings});
            }

            return timersSettings;
        }

        public async Task UpdateAsync(TimersSettings timersSettings, string userId)
        {
            await _timersSettingsRepository.InsertOrReplaceAsync(timersSettings);

            _cache.Set(timersSettings);
        }
    }
}
