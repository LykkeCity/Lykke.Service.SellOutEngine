using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Trades
{
    [UsedImplicitly]
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public Task<IReadOnlyCollection<Trade>> GetTradesAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return _tradeRepository.GetAsync(startDate, endDate, limit);
        }

        public Task<Trade> GetTradeByIdAsync(string tradeId)
        {
            return _tradeRepository.GetByIdAsync(tradeId);
        }

        public Task RegisterAsync(Trade trade)
        {
            return _tradeRepository.InsertAsync(trade);
        }
    }
}
