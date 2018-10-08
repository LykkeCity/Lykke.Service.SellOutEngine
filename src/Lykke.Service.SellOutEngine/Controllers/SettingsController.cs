using System.Net;
using System.Threading.Tasks;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.SellOutEngine.Client.Api;
using Lykke.Service.SellOutEngine.Client.Models.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.SellOutEngine.Controllers
{
    [Route("/api/[controller]")]
    public class SettingsController : Controller, ISettingsApi
    {
        public SettingsController()
        {
        }

        /// <response code="200">The settings of service account.</response>
        [HttpGet("account")]
        [ProducesResponseType(typeof(TimersSettingsModel), (int) HttpStatusCode.OK)]
        public Task<AccountSettingsModel> GetAccountSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <response code="200">The settings of service timers.</response>
        [HttpGet("timers")]
        [ProducesResponseType(typeof(TimersSettingsModel), (int) HttpStatusCode.OK)]
        public Task<TimersSettingsModel> GetTimersSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">The settings of service timers successfully saved.</response>
        [HttpPost("timers")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task SaveTimersSettingsAsync([FromBody] TimersSettingsModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            throw new System.NotImplementedException();
        }

        /// <response code="200">The settings of quotes timeouts.</response>
        [HttpGet("quotes")]
        [ProducesResponseType(typeof(QuoteTimeoutSettingsModel), (int) HttpStatusCode.OK)]
        public Task<QuoteTimeoutSettingsModel> GetQuoteTimeoutSettingsAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <response code="204">The settings of quotes timeouts successfully saved.</response>
        [HttpPost("quotes")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public Task SaveQuoteTimeoutSettingsAsync([FromBody] QuoteTimeoutSettingsModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationApiException("Used id required");

            throw new System.NotImplementedException();
        }
    }
}
