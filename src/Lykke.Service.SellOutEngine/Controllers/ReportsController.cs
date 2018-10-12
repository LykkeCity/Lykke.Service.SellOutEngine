using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Reports;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class ReportsController : Controller, IReportsApi
    {
        private readonly ISummaryReportService _summaryReportService;

        public ReportsController(ISummaryReportService summaryReportService)
        {
            _summaryReportService = summaryReportService;
        }

        /// <inheritdoc/>
        /// <response code="200">A collection of asset pair summary info.</response>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(IReadOnlyCollection<SummaryReportModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<SummaryReportModel>> GetSummaryReportAsync()
        {
            IReadOnlyCollection<SummaryReport> summaryReports = await _summaryReportService.GetAllAsync();

            return Mapper.Map<SummaryReportModel[]>(summaryReports);
        }
    }
}
