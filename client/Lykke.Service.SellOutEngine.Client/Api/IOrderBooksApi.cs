using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.OrderBooks;
using Refit;

namespace Lykke.Service.SellOutEngine.Client.Api
{
    /// <summary>
    /// Provides methods to work with order books.
    /// </summary>
    [PublicAPI]
    public interface IOrderBooksApi
    {
        /// <summary>
        /// Returns a collection of order books.
        /// </summary>
        /// <returns>A collection of order books.</returns>
        [Get("/api/orderbooks")]
        Task<IReadOnlyCollection<OrderBookModel>> GetAllAsync();

        /// <summary>
        /// Returns an order book by asset pair.
        /// </summary>
        /// <param name="assetPairId">The identifier of the asset pair.</param>
        /// <returns>An order book.</returns>
        [Get("/api/orderbooks/{assetPairId}")]
        Task<OrderBookModel> GetByAssetPairAsync(string assetPairId);
    }
}
