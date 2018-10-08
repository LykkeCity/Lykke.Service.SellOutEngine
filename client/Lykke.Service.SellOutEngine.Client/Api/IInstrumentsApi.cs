using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Instruments;
using Refit;

namespace Lykke.Service.SellOutEngine.Client.Api
{
    /// <summary>
    /// Provides methods to work with instruments.
    /// </summary>
    [PublicAPI]
    public interface IInstrumentsApi
    {
        /// <summary>
        /// Returns a collection of instruments.
        /// </summary>
        /// <returns>A collection of instruments.</returns>
        [Get("/api/instruments")]
        Task<IReadOnlyCollection<InstrumentModel>> GetAllAsync();

        /// <summary>
        /// Returns an instrument by asset pair id.
        /// </summary>
        /// <param name="assetPairId">The identifier of the asset pair.</param>
        /// <returns>The instrument.</returns>
        [Get("/api/instruments/{assetPairId}")]
        Task<InstrumentModel> GetByAssetPairAsync(string assetPairId);

        /// <summary>
        /// Adds new instrument.
        /// </summary>
        /// <param name="model">The model that describes instrument.</param>
        /// <param name="userId">The identifier of the user that execute the operation.</param>
        [Post("/api/instruments")]
        Task AddAsync([Body] InstrumentModel model, string userId);

        /// <summary>
        /// Updates existing instrument.
        /// </summary>
        /// <param name="model">The model that describes instrument.</param>
        /// <param name="userId">The identifier of the user that execute the operation.</param>
        [Put("/api/instruments")]
        Task UpdateAsync([Body] InstrumentModel model, string userId);

        /// <summary>
        /// Deletes instrument by asset pair id.
        /// </summary>
        /// <param name="assetPairId">The identifier of the asset pair.</param>
        /// <param name="userId">The identifier of the user that execute the operation.</param>
        [Delete("/api/instruments/{assetPairId}")]
        Task DeleteAsync(string assetPairId, string userId);
    }
}
