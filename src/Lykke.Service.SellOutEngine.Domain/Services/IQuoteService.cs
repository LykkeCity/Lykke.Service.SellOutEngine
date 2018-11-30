using System.Collections.Generic;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IQuoteService
    {
        Quote Get(string source, string assetPairId);

        IReadOnlyCollection<string> GetSources();
        
        void Set(Quote quote);
    }
}
