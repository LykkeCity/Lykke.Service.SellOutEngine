using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Balances.AutorestClient.Models;
using Lykke.Service.Balances.Client;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Balances
{
    [UsedImplicitly]
    public class BalanceService : IBalanceService
    {
        private readonly ISettingsService _settingsService;
        private readonly IBalancesClient _balancesClient;
        private readonly ILog _log;
        private readonly InMemoryCache<Balance> _cache;

        public BalanceService(
            ISettingsService settingsService,
            IBalancesClient balancesClient,
            ILogFactory logFactory)
        {
            _settingsService = settingsService;
            _balancesClient = balancesClient;
            _log = logFactory.CreateLog(this);
            _cache = new InMemoryCache<Balance>(balance => balance.AssetId, true);
        }

        public Task<IReadOnlyCollection<Balance>> GetAsync()
        {
            return Task.FromResult(_cache.GetAll());
        }

        public Task<Balance> GetByAssetIdAsync(string assetId)
        {
            return Task.FromResult(_cache.Get(assetId) ?? new Balance(assetId, decimal.Zero, decimal.Zero));
        }

        public async Task UpdateBalancesAsync()
        {
            string walletId = await _settingsService.GetWalletIdAsync();

            if (string.IsNullOrEmpty(walletId))
                return;

            try
            {
                IEnumerable<ClientBalanceResponseModel> balances =
                    await _balancesClient.GetClientBalances(walletId);

                _cache.Set(balances.Select(o => new Balance(o.AssetId, o.Balance, o.Reserved)).ToArray());
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred while getting balances from Lykke exchange.");
            }
        }
    }
}
