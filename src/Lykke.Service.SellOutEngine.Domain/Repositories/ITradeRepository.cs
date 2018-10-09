using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.SellOutEngine.Domain.Repositories
{
    public interface ITradeRepository
    {
        Task<IReadOnlyCollection<Trade>> GetAsync(DateTime startDate, DateTime endDate, int limit);

        Task<Trade> GetByIdAsync(string internalTradeId);

        Task InsertAsync(Trade internalTrade);
    }
}
