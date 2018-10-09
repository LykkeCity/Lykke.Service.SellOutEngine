using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface ITimersSettingsService
    {
        Task<TimersSettings> GetAsync();

        Task UpdateAsync(TimersSettings timersSettings, string userId);
    }
}
