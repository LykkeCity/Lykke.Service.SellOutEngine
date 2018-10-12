using System;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Db;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.SellOutEngine.Settings.ServiceSettings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SellOutEngineSettings
    {
        public string WalletId { get; set; }

        public TimeSpan AssetsCacheExpirationPeriod { get; set; }

        public string[] QuoteSources { get; set; }

        public DbSettings Db { get; set; }

        public RabbitSettings Rabbit { get; set; }
    }
}
