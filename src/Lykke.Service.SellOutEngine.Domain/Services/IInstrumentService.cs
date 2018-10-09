using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface IInstrumentService
    {
        Task<IReadOnlyCollection<Instrument>> GetAllAsync();

        Task<Instrument> GetByAssetPairIdAsync(string assetPairId);

        Task AddAsync(Instrument instrument, string userId);

        Task UpdateAsync(Instrument instrument, string userId);

        Task DeleteAsync(string assetPairId, string userId);
    }
}
