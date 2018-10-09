using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Services
{
    public interface ITradeService
    {
        Task<IReadOnlyCollection<Trade>> GetTradesAsync(DateTime startDate, DateTime endDate, int limit);

        Task<Trade> GetTradeByIdAsync(string tradeId);

        Task RegisterAsync(Trade trade);
    }
}
