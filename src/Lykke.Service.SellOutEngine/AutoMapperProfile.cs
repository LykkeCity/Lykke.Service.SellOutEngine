using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Balances;
using Lykke.Service.SellOutEngine.Client.Models.Instruments;
using Lykke.Service.SellOutEngine.Client.Models.OrderBooks;
using Lykke.Service.SellOutEngine.Client.Models.Reports;
using Lykke.Service.SellOutEngine.Client.Models.Settings;
using Lykke.Service.SellOutEngine.Client.Models.Trades;
using Lykke.Service.SellOutEngine.Domain;

namespace Lykke.Service.SellOutEngine
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Balance, BalanceModel>(MemberList.Source);

            CreateMap<Instrument, InstrumentModel>(MemberList.Source);
            CreateMap<InstrumentModel, Instrument>(MemberList.Destination);

            CreateMap<LimitOrder, LimitOrderModel>(MemberList.Source);

            CreateMap<OrderBook, OrderBookModel>(MemberList.Source);

            CreateMap<SummaryReport, SummaryReportModel>(MemberList.Source);

            CreateMap<QuoteTimeoutSettings, QuoteTimeoutSettingsModel>(MemberList.Source);
            CreateMap<QuoteTimeoutSettingsModel, QuoteTimeoutSettings>(MemberList.Destination);

            CreateMap<TimersSettings, TimersSettingsModel>(MemberList.Source);
            CreateMap<TimersSettingsModel, TimersSettings>(MemberList.Destination);

            CreateMap<Trade, TradeModel>(MemberList.Source);
        }
    }
}
