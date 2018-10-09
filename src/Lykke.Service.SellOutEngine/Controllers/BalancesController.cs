using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Balances;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class BalancesController : Controller, IBalancesApi
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of balances.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<BalanceModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<BalanceModel>> GetAllAsync()
        {
            IReadOnlyCollection<Balance> balances = await _balanceService.GetAsync();

            return Mapper.Map<BalanceModel[]>(balances);
        }

        /// <inheritdoc/>
        /// <response code="200">The balance of asset.</response>
        [HttpGet("{assetId}")]
        [ProducesResponseType(typeof(BalanceModel), (int) HttpStatusCode.OK)]
        public async Task<BalanceModel> GetByAssetIdAsync(string assetId)
        {
            Balance balance = await _balanceService.GetByAssetIdAsync(assetId);

            return Mapper.Map<BalanceModel>(balance);
        }
    }
}
