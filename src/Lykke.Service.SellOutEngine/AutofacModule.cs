using Autofac;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.SellOutEngine.Managers;
using Lykke.Service.SellOutEngine.Settings;
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
            builder.RegisterModule(new DomainServices.AutofacModule());
            builder.RegisterModule(new AzureRepositories.AutofacModule(
                _settings.Nested(o => o.SellOutEngineService.Db.DataConnectionString)));

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
        }
    }
}
