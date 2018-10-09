using System.Collections.Generic;
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
        private readonly IReadOnlyCollection<string> _quoteSources;

        public AutofacModule(string walletId, IReadOnlyCollection<string> quoteSources)
        {
            _walletId = walletId;
            _quoteSources = quoteSources;
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
                .WithParameter(new NamedParameter("quoteSources", _quoteSources))
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

            builder.RegisterType<BalancesTimer>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
