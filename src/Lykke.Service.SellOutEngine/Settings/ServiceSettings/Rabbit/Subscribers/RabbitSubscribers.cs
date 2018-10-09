using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class RabbitSubscribers
    {
        public SubscriberSettings LykkeTrades { get; set; }

        public QuoteSubscriberSettings Quotes { get; set; }
    }
}
