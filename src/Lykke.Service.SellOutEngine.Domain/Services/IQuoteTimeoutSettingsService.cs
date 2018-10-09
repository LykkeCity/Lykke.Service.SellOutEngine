using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IQuoteTimeoutSettingsService
    {
        Task<QuoteTimeoutSettings> GetAsync();

        Task UpdateAsync(QuoteTimeoutSettings quoteTimeoutSettings, string userId);
    }
}
