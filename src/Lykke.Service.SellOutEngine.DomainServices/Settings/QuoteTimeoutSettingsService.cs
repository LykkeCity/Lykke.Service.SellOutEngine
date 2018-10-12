using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Settings
{
    [UsedImplicitly]
    public class QuoteTimeoutSettingsService : IQuoteTimeoutSettingsService
    {
        private const string CacheKey = "key";

        private readonly IQuoteTimeoutSettingsRepository _quoteTimeoutSettingsRepository;
        private readonly InMemoryCache<QuoteTimeoutSettings> _cache;

        public QuoteTimeoutSettingsService(IQuoteTimeoutSettingsRepository quoteTimeoutSettingsRepository)
        {
            _quoteTimeoutSettingsRepository = quoteTimeoutSettingsRepository;
            _cache = new InMemoryCache<QuoteTimeoutSettings>(settings => CacheKey, true);
        }

        public async Task<QuoteTimeoutSettings> GetAsync()
        {
            QuoteTimeoutSettings quoteTimeoutSettings = _cache.Get(CacheKey);

            if (quoteTimeoutSettings == null)
            {
                quoteTimeoutSettings = await _quoteTimeoutSettingsRepository.GetAsync();

                if (quoteTimeoutSettings == null)
                {
                    quoteTimeoutSettings = new QuoteTimeoutSettings
                    {
                        Enabled = true,
                        Value = TimeSpan.FromSeconds(5)
                    };
                }

                _cache.Initialize(new[] {quoteTimeoutSettings});
            }

            return quoteTimeoutSettings;
        }

        public async Task UpdateAsync(QuoteTimeoutSettings quoteTimeoutSettings, string userId)
        {
            await _quoteTimeoutSettingsRepository.InsertOrReplaceAsync(quoteTimeoutSettings);

            _cache.Set(quoteTimeoutSettings);
        }
    }
}
