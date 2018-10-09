using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Repositories
{
    public interface ITimersSettingsRepository
    {
        Task<TimersSettings> GetAsync();

        Task InsertOrReplaceAsync(TimersSettings timersSettings);
    }
}
