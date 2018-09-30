using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Db;

namespace Lykke.Service.SellOutEngine.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SellOutEngineSettings
    {
        public DbSettings Db { get; set; }
    }
}
