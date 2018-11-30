using System.Net;
using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Assets.Client;
using Lykke.Service.Balances.Client;
using Lykke.Service.SellOutEngine.Managers;
using Lykke.Service.SellOutEngine.Rabbit.Subscribers;
using Lykke.Service.SellOutEngine.Settings;
using Lykke.Service.SellOutEngine.Settings.Clients.MatchingEngine;
using Lykke.Service.SellOutEngine.Settings.ServiceSettings.Rabbit.Subscribers;
using Lykke.SettingsReader;

namespace Lykke.Service.SellOutEngine
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public AutofacModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DomainServices.AutofacModule(
                _settings.CurrentValue.SellOutEngineService.WalletId,
                _settings.CurrentValue.SellOutEngineService.QuoteSources));
            builder.RegisterModule(new AzureRepositories.AutofacModule(
                _settings.Nested(o => o.SellOutEngineService.Db.DataConnectionString),
                _settings.Nested(o => o.SellOutEngineService.Db.LykkeTradesMeQueuesDeduplicatorConnectionString)));

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            RegisterClients(builder);

            RegisterRabbit(builder);
        }

        private void RegisterClients(ContainerBuilder builder)
        {
            builder.RegisterAssetsClient(_settings.CurrentValue.AssetsServiceClient);

            builder.RegisterBalancesClient(_settings.CurrentValue.BalancesServiceClient);

            MatchingEngineClientSettings matchingEngineClientSettings = _settings.CurrentValue.MatchingEngineClient;

            if (!IPAddress.TryParse(matchingEngineClientSettings.IpEndpoint.Host, out var address))
                address = Dns.GetHostAddressesAsync(matchingEngineClientSettings.IpEndpoint.Host).Result[0];

            var endPoint = new IPEndPoint(address, matchingEngineClientSettings.IpEndpoint.Port);

            builder.RegisgterMeClient(endPoint);
        }

        private void RegisterRabbit(ContainerBuilder builder)
        {
            builder.RegisterType<LykkeTradeSubscriber>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.SellOutEngineService.Rabbit.Subscribers
                    .LykkeTrades))
                .AsSelf()
                .SingleInstance();

            QuoteSubscriberSettings quoteSubscriberSettings =
                _settings.CurrentValue.SellOutEngineService.Rabbit.Subscribers.Quotes;

            foreach (string exchange in quoteSubscriberSettings.Exchanges)
            {
                builder.RegisterType<QuoteSubscriber>()
                    .AsSelf()
                    .WithParameter(TypedParameter.From(new SubscriberSettings
                    {
                        Exchange = exchange,
                        Queue = quoteSubscriberSettings.Queue,
                        ConnectionString = quoteSubscriberSettings.ConnectionString
                    }))
                    .SingleInstance();
            }
        }
    }
}
