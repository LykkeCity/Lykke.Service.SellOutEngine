using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IQuoteService
    {
        Task<Quote> GetAsync(string source, string assetPairId);

        Task SetAsync(Quote quote);
    }
}
