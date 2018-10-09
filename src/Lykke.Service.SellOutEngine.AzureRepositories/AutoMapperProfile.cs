using AutoMapper;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.AzureRepositories.Instruments;
using Lykke.Service.SellOutEngine.AzureRepositories.Reports;
using Lykke.Service.SellOutEngine.AzureRepositories.Settings;
using Lykke.Service.SellOutEngine.AzureRepositories.Trades;
using Lykke.Service.SellOutEngine.Domain;

namespace Lykke.Service.SellOutEngine.AzureRepositories
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Instrument, InstrumentEntity>(MemberList.Source);
            CreateMap<InstrumentEntity, Instrument>(MemberList.Destination);

            CreateMap<SummaryReport, SummaryReportEntity>(MemberList.Source);
            CreateMap<SummaryReportEntity, SummaryReport>(MemberList.Destination);

            CreateMap<QuoteTimeoutSettings, QuoteTimeoutSettingsEntity>(MemberList.Source);
            CreateMap<QuoteTimeoutSettingsEntity, QuoteTimeoutSettings>(MemberList.Destination);

            CreateMap<TimersSettings, TimersSettingsEntity>(MemberList.Source);
            CreateMap<TimersSettingsEntity, TimersSettings>(MemberList.Destination);

            CreateMap<Trade, TradeEntity>(MemberList.Source);
            CreateMap<TradeEntity, Trade>(MemberList.Destination);
        }
    }
}
