using Autofac;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Domain.Services;
using Lykke.Service.SellOutEngine.DomainServices.Balances;
using Lykke.Service.SellOutEngine.DomainServices.Exchanges;
using Lykke.Service.SellOutEngine.DomainServices.Instruments;
using Lykke.Service.SellOutEngine.DomainServices.OrderBooks;
using Lykke.Service.SellOutEngine.DomainServices.Reports;
using Lykke.Service.SellOutEngine.DomainServices.Settings;
using Lykke.Service.SellOutEngine.DomainServices.Timers;
using Lykke.Service.SellOutEngine.DomainServices.Trades;

namespace Lykke.Service.SellOutEngine.DomainServices
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly string _walletId;

        public AutofacModule(string walletId)
        {
            _walletId = walletId;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BalanceService>()
                .As<IBalanceService>()
                .SingleInstance();

            builder.RegisterType<LykkeExchangeService>()
                .As<ILykkeExchangeService>()
                .SingleInstance();

            builder.RegisterType<InstrumentService>()
                .As<IInstrumentService>()
                .SingleInstance();

            builder.RegisterType<OrderBookService>()
                .As<IOrderBookService>()
                .SingleInstance();

            builder.RegisterType<QuoteService>()
                .As<IQuoteService>()
                .SingleInstance();

            builder.RegisterType<SummaryReportService>()
                .As<ISummaryReportService>()
                .SingleInstance();

            builder.RegisterType<SettingsService>()
                .As<ISettingsService>()
                .WithParameter(new NamedParameter("walletId", _walletId))
                .SingleInstance();

            builder.RegisterType<TimersSettingsService>()
                .As<ITimersSettingsService>()
                .SingleInstance();

            builder.RegisterType<QuoteTimeoutSettingsService>()
                .As<IQuoteTimeoutSettingsService>()
                .SingleInstance();

            builder.RegisterType<TradeService>()
                .As<ITradeService>()
                .SingleInstance();

            builder.RegisterType<MarketMakerService>()
                .As<IMarketMakerService>()
                .SingleInstance();


            builder.RegisterType<BalancesTimer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<MarketMakerTimer>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
