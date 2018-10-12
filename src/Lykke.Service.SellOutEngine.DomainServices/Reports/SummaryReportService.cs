using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Lykke.Service.SellOutEngine.Domain.Services;

namespace Lykke.Service.SellOutEngine.DomainServices.Reports
{
    [UsedImplicitly]
    public class SummaryReportService : ISummaryReportService
    {
        private readonly ISummaryReportRepository _summaryReportRepository;
        private readonly InMemoryCache<SummaryReport> _cache;

        public SummaryReportService(ISummaryReportRepository summaryReportRepository, ILogFactory logFactory)
        {
            _summaryReportRepository = summaryReportRepository;
            _cache = new InMemoryCache<SummaryReport>(summaryReport => summaryReport.AssetPairId, false);
        }

        public async Task<IReadOnlyCollection<SummaryReport>> GetAllAsync()
        {
            IReadOnlyCollection<SummaryReport> summaryReports = _cache.GetAll();

            if (summaryReports == null)
            {
                summaryReports = await _summaryReportRepository.GetAllAsync();

                _cache.Initialize(summaryReports);
            }

            return summaryReports;
        }

        public async Task RegisterTradeAsync(Trade trade)
        {
            IReadOnlyCollection<SummaryReport> summaryReports = await GetAllAsync();

            SummaryReport summaryReport = summaryReports.SingleOrDefault(o => o.AssetPairId == trade.AssetPairId);

            bool isNew = false;

            if (summaryReport == null)
            {
                summaryReport = new SummaryReport {AssetPairId = trade.AssetPairId};
                isNew = true;
            }

            summaryReport.ApplyTrade(trade);

            if (isNew)
                await _summaryReportRepository.InsertAsync(summaryReport);
            else
                await _summaryReportRepository.UpdateAsync(summaryReport);

            _cache.Set(summaryReport);
        }
    }
}
