using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Trades;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class TradesController : Controller, ITradesApi
    {
        private readonly ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of trades.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<TradeModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<TradeModel>> GetByPeriodAsync(DateTime startDate, DateTime endDate,
            int limit)
        {
            IReadOnlyCollection<Trade> externalTrades =
                await _tradeService.GetTradesAsync(startDate, endDate, limit);

            return Mapper.Map<TradeModel[]>(externalTrades);
        }

        /// <inheritdoc/>
        /// <response code="200">A trade.</response>
        /// <response code="404">Trade does not exist.</response>
        [HttpGet("{tradeId}")]
        [ProducesResponseType(typeof(TradeModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<TradeModel> GetByIdAsync(string tradeId)
        {
            Trade trade = await _tradeService.GetTradeByIdAsync(tradeId);

            if (trade == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Trade does not exist.");

            return Mapper.Map<TradeModel>(trade);
        }
    }
}
