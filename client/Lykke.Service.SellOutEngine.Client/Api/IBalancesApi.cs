using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Balances;
using Refit;

namespace Lykke.Service.SellOutEngine.Client.Api
{
    /// <summary>
    /// Provides methods to work with balances.
    /// </summary>
    [PublicAPI]
    public interface IBalancesApi
    {
        /// <summary>
        /// Returns a collection of balances.
        /// </summary>
        /// <returns>A collection of balances</returns>
        [Get("/api/balances")]
        Task<IReadOnlyCollection<BalanceModel>> GetAllAsync();

        /// <summary>
        /// Returns a balance of the asset.
        /// </summary>
        /// <param name="assetId">The identifier of the asset.</param>
        /// <returns>A balance of the asset.</returns>
        [Get("/api/balances/{assetId}")]
        Task<BalanceModel> GetByAssetIdAsync(string assetId);
    }
}
