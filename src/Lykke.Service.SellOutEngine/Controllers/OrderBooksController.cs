using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.OrderBooks;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class OrderBooksController : Controller, IOrderBooksApi
    {
        public OrderBooksController()
        {
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of order books.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<OrderBookModel>), (int) HttpStatusCode.OK)]
        public Task<IReadOnlyCollection<OrderBookModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="200">An order book.</response>
        /// <response code="404">Order book does not exist.</response>
        [HttpGet("{assetPairId}")]
        [ProducesResponseType(typeof(OrderBookModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public Task<OrderBookModel> GetByAssetPairAsync(string assetPairId)
        {
            throw new System.NotImplementedException();
        }
    }
}
