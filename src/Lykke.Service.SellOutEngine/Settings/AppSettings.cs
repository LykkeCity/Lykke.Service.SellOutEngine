using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings;

namespace Lykke.Service.SellOutEngine.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public SellOutEngineSettings SellOutEngineService { get; set; }
    }
}
