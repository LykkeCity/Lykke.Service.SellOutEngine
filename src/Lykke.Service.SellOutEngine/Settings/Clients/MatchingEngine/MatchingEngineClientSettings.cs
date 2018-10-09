using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Settings.Clients.MatchingEngine
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MatchingEngineClientSettings
    {
        public IpEndpointSettings IpEndpoint { get; set; }
    }
}
