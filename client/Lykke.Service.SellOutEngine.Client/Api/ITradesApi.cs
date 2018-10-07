using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Trades;
using Refit;

namespace Lykke.Service.SellOutEngine.Client.Api
{
    /// <summary>
    /// Provides methods to work with trades.
    /// </summary>
    [PublicAPI]
    public interface ITradesApi
    {
        /// <summary>
        /// Returns a collection of trades by period.
        /// </summary>
        /// <param name="startDate">The start date of period.</param>
        /// <param name="endDate">The end date of period.</param>
        /// <param name="limit">The maximum number of trades.</param>
        /// <returns>A collection of trades.</returns>
        [Get("/api/trades")]
        Task<IReadOnlyCollection<TradeModel>> GetByPeriodAsync(DateTime startDate, DateTime endDate, int limit);

        /// <summary>
        /// Returns a trade by id.
        /// </summary>
        /// <param name="tradeId">The identifier of trade.</param>
        /// <returns>A trade.</returns>
        [Get("/api/trades/{tradeId}")]
        Task<TradeModel> GetByIdAsync(string tradeId);
    }
}
