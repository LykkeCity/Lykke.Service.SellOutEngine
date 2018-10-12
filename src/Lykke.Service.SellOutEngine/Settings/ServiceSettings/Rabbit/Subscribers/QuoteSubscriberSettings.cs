using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class QuoteSubscriberSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string[] Exchanges { get; set; }

        public string Queue { get; set; }
    }
}
