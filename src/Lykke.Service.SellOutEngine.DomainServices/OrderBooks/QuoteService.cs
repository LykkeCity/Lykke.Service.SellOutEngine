using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.OrderBooks
{
    [UsedImplicitly]
    public class QuoteService : IQuoteService
    {
        private readonly InMemoryCache<Quote> _cache;

        private readonly HashSet<string> _sources = new HashSet<string>();

        public QuoteService()
        {
            _cache = new InMemoryCache<Quote>(quote => $"{quote.AssetPairId}-{quote.Source}", true);
        }

        public Quote Get(string source, string assetPairId)
        {
            return _cache.Get($"{assetPairId}-{source}");
        }

        public IReadOnlyCollection<string> GetSources()
        {
            return _sources.ToArray();
        }

        public void Set(Quote quote)
        {
            _cache.Set(quote);

            _sources.Add(quote.Source);
        }
    }
}
