using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Settings;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class SettingsController : Controller, ISettingsApi
    {
        private readonly ISettingsService _settingsService;
        private readonly ITimersSettingsService _timersSettingsService;
        private readonly IQuoteTimeoutSettingsService _quoteTimeoutSettingsService;

        public SettingsController(
            ISettingsService settingsService,
            ITimersSettingsService timersSettingsService,
            IQuoteTimeoutSettingsService quoteTimeoutSettingsService)
        {
            _settingsService = settingsService;
            _timersSettingsService = timersSettingsService;
            _quoteTimeoutSettingsService = quoteTimeoutSettingsService;
        }

        /// <response code="200">The settings of service account.</response>
        [HttpGet("account")]
        [ProducesResponseType(typeof(AccountSettingsModel), (int) HttpStatusCode.OK)]
        public async Task<AccountSettingsModel> GetAccountSettingsAsync()
        {
            string walletId = await _settingsService.GetWalletIdAsync();

            return new AccountSettingsModel {WalletId = walletId};
        }

        /// <response code="200">A collection of quote sources.</response>
        [HttpGet("quotesources")]
        [ProducesResponseType(typeof(IReadOnlyCollection<string>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<string>> GetQuoteSourcesAsync()
        {
            return await _settingsService.GetQuoteSourcesAsync();
        }

        /// <response code="200">The settings of service timers.</response>
        [HttpGet("timers")]
        [ProducesResponseType(typeof(TimersSettingsModel), (int) HttpStatusCode.OK)]
        public async Task<TimersSettingsModel> GetTimersSettingsAsync()
        {
            TimersSettings timersSettings = await _timersSettingsService.GetAsync();

            return Mapper.Map<TimersSettingsModel>(timersSettings);
        }

        /// <response code="204">The settings of service timers successfully saved.</response>
        [HttpPost("timers")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task SaveTimersSettingsAsync([FromBody] TimersSettingsModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            var timersSettings = Mapper.Map<TimersSettings>(model);

            await _timersSettingsService.UpdateAsync(timersSettings, userId);
        }

        /// <response code="200">The settings of quotes timeouts.</response>
        [HttpGet("quotes")]
        [ProducesResponseType(typeof(QuoteTimeoutSettingsModel), (int) HttpStatusCode.OK)]
        public async Task<QuoteTimeoutSettingsModel> GetQuoteTimeoutSettingsAsync()
        {
            QuoteTimeoutSettings quoteTimeoutSettings = await _quoteTimeoutSettingsService.GetAsync();

            return Mapper.Map<QuoteTimeoutSettingsModel>(quoteTimeoutSettings);
        }

        /// <response code="204">The settings of quotes timeouts successfully saved.</response>
        [HttpPost("quotes")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task SaveQuoteTimeoutSettingsAsync([FromBody] QuoteTimeoutSettingsModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            var quoteTimeoutSettings = Mapper.Map<QuoteTimeoutSettings>(model);

            await _quoteTimeoutSettingsService.UpdateAsync(quoteTimeoutSettings, userId);
        }
    }
}
