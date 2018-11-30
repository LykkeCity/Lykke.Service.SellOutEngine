using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMq.Azure.Deduplicator;
using Lykke.RabbitMqBroker.Deduplication;
using Lykke.Service.SellOutEngine.AzureRepositories.Instruments;
using Lykke.Service.SellOutEngine.AzureRepositories.Reports;
using Lykke.Service.SellOutEngine.AzureRepositories.Settings;
using Lykke.Service.SellOutEngine.AzureRepositories.Trades;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.SellOutEngine.AzureRepositories
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<string> _connectionString;
        private readonly IReloadingManager<string> _lykkeTradesDeduplicatorConnectionString;

        public AutofacModule(
            IReloadingManager<string> connectionString,
            IReloadingManager<string> lykkeTradesDeduplicatorConnectionString)
        {
            _connectionString = connectionString;
            _lykkeTradesDeduplicatorConnectionString = lykkeTradesDeduplicatorConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(container => new InstrumentRepository(
                    AzureTableStorage<InstrumentEntity>.Create(_connectionString,
                        "Instruments", container.Resolve<ILogFactory>())))
                .As<IInstrumentRepository>()
                .SingleInstance();

            builder.Register(container => new SummaryReportRepository(
                    AzureTableStorage<SummaryReportEntity>.Create(_connectionString,
                        "SummaryReports", container.Resolve<ILogFactory>())))
                .As<ISummaryReportRepository>()
                .SingleInstance();

            builder.Register(container => new TimersSettingsRepository(
                    AzureTableStorage<TimersSettingsEntity>.Create(_connectionString,
                        "Settings", container.Resolve<ILogFactory>())))
                .As<ITimersSettingsRepository>()
                .SingleInstance();

            builder.Register(container => new QuoteTimeoutSettingsRepository(
                    AzureTableStorage<QuoteTimeoutSettingsEntity>.Create(_connectionString,
                        "Settings", container.Resolve<ILogFactory>())))
                .As<IQuoteTimeoutSettingsRepository>()
                .SingleInstance();

            builder.Register(container => new TradeRepository(
                    AzureTableStorage<TradeEntity>.Create(_connectionString,
                        "InternalTrades", container.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_connectionString,
                        "InternalTradesIndices", container.Resolve<ILogFactory>())))
                .As<ITradeRepository>()
                .SingleInstance();

            builder.Register(container => new AzureStorageDeduplicator(
                    AzureTableStorage<DuplicateEntity>.Create(_lykkeTradesDeduplicatorConnectionString,
                        "LykkeTradesDeduplicator", container.Resolve<ILogFactory>())))
                .As<IDeduplicator>()
                .SingleInstance();
        }
    }
}
