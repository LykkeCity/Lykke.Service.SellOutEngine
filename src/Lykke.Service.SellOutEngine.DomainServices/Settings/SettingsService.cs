using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Settings
{
    [UsedImplicitly]
    public class SettingsService : ISettingsService
    {
        private readonly string _walletId;
        private readonly IReadOnlyCollection<string> _quoteSources;

        public SettingsService(string walletId, IReadOnlyCollection<string> quoteSources)
        {
            _walletId = walletId;
            _quoteSources = quoteSources;
        }

        public Task<string> GetWalletIdAsync()
        {
            return Task.FromResult(_walletId);
        }

        public Task<IReadOnlyCollection<string>> GetQuoteSourcesAsync()
        {
            return Task.FromResult(_quoteSources);
        }
    }
}
