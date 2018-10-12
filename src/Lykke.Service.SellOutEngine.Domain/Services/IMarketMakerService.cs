using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IMarketMakerService
    {
        Task UpdateOrderBooksAsync();
    }
}
