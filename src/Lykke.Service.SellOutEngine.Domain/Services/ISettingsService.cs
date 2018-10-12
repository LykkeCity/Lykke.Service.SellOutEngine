using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface ISettingsService
    {
        Task<string> GetWalletIdAsync();

        Task<IReadOnlyCollection<string>> GetQuoteSourcesAsync();
    }
}
