using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Assets.Client;
using Lykke.Service.Balances.Client;
using Lykke.Service.SellOutEngine.Settings.Clients;
using Lykke.Service.SellOutEngine.Settings.Clients.MatchingEngine;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings;

namespace Lykke.Service.SellOutEngine.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public SellOutEngineSettings SellOutEngineService { get; set; }

        public AssetServiceSettings AssetsServiceClient { get; set; }

        public BalancesServiceClientSettings BalancesServiceClient { get; set; }

        public MatchingEngineClientSettings MatchingEngineClient { get; set; }
    }
}
