using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.OrderBooks;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class OrderBooksController : Controller, IOrderBooksApi
    {
        private readonly IOrderBookService _orderBookService;

        public OrderBooksController(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of order books.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<OrderBookModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<OrderBookModel>> GetAllAsync()
        {
            IReadOnlyCollection<OrderBook> orderBooks = await _orderBookService.GetAllAsync();

            return Mapper.Map<OrderBookModel[]>(orderBooks);
        }

        /// <inheritdoc/>
        /// <response code="200">An order book.</response>
        /// <response code="404">Order book does not exist.</response>
        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(OrderBookModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<OrderBookModel> GetByAssetPairAsync(string assetPairId)
        {
            OrderBook orderBook = await _orderBookService.GetByAssetPairIdAsync(assetPairId);

            if (orderBook == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Order book does not exist.");

            return Mapper.Map<OrderBookModel>(orderBook);
        }
    }
}
