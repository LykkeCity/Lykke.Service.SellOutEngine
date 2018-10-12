using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IOrderBookService
    {
        Task<IReadOnlyCollection<OrderBook>> GetAllAsync();

        Task<OrderBook> GetByAssetPairIdAsync(string assetPairId);

        Task UpdateAsync(OrderBook orderBook);

        Task RemoveAsync(string assetPairId);
    }
}
