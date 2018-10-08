using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Balances;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class BalancesController : Controller, IBalancesApi
    {
        public BalancesController()
        {
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of balances.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<BalanceModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<BalanceModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <response code="200">The balance of asset.</response>
        [HttpGet("{assetId}")]
        [ProducesResponseType(typeof(BalanceModel), (int) HttpStatusCode.OK)]
        public async Task<BalanceModel> GetByAssetIdAsync(string assetId)
        {
            throw new NotImplementedException();
        }
    }
}
