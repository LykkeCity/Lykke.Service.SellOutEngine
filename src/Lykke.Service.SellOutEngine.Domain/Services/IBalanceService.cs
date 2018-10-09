using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IBalanceService
    {
        Task<IReadOnlyCollection<Balance>> GetAsync();

        Task<Balance> GetByAssetIdAsync(string assetId);

        Task UpdateBalancesAsync();
    }
}
