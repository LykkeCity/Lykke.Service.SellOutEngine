using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface ILykkeExchangeService
    {
        Task ApplyAsync(string assetPairId, IReadOnlyCollection<LimitOrder> limitOrders);
    }
}
