using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface ISummaryReportService
    {
        Task<IReadOnlyCollection<SummaryReport>> GetAllAsync();

        Task RegisterTradeAsync(Trade trade);
    }
}
