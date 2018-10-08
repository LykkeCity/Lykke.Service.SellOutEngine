using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Instruments;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class InstrumentsController : Controller, IInstrumentsApi
    {
        public InstrumentsController()
        {
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of instruments.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<InstrumentModel>), (int) HttpStatusCode.OK)]
        public Task<IReadOnlyCollection<InstrumentModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="200">An instrument.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(InstrumentModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public Task<InstrumentModel> GetByAssetPairAsync(string assetPairId)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully added.</response>
        /// <response code="409">Instrument already exists.</response>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.Conflict)]
        public Task AddAsync([FromBody] InstrumentModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully updated.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public Task UpdateAsync([FromBody] InstrumentModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="204">The instrument successfully deleted.</response>
        /// <response code="404">Instrument does not exist.</response>
        [HttpDelete("{assetPairId}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public Task DeleteAsync(string assetPairId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            throw new System.NotImplementedException();
        }
    }
}
