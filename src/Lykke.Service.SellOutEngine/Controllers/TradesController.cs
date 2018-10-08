using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Trades;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class TradesController : Controller, ITradesApi
    {
        public TradesController()
        {
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of trades.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<TradeModel>), (int) HttpStatusCode.OK)]
        public Task<IReadOnlyCollection<TradeModel>> GetByPeriodAsync(DateTime startDate, DateTime endDate, int limit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="200">An external trade.</response>
        /// <response code="404">External trade does not exist.</response>
        [HttpGet("{tradeId}")]
        [ProducesResponseType(typeof(TradeModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public Task<TradeModel> GetByIdAsync(string tradeId)
        {
            throw new NotImplementedException();
        }
    }
}
