using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Repositories
{
    public interface IQuoteTimeoutSettingsRepository
    {
        Task<QuoteTimeoutSettings> GetAsync();

        Task InsertOrReplaceAsync(QuoteTimeoutSettings quoteTimeoutSettings);
    }
}
