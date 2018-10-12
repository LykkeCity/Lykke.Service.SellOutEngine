using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers;

namespace Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSettings
    {
        public RabbitSubscribers Subscribers { get; set; }
    }
}
