using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.OrderBooks
{
    [UsedImplicitly]
    public class QuoteService : IQuoteService
    {
        private readonly InMemoryCache<Quote> _cache;

        public QuoteService()
        {
            _cache = new InMemoryCache<Quote>(quote => $"{quote.AssetPairId}-{quote.Source}", true);
        }

        public Task SetAsync(Quote quote)
        {
            _cache.Set(quote);

            return Task.CompletedTask;
        }

        public Task<Quote> GetAsync(string source, string assetPairId)
        {
            return Task.FromResult(_cache.Get($"{assetPairId}-{source}"));
        }
    }
}
