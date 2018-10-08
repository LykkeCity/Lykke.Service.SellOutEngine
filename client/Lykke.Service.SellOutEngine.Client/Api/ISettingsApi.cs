using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Settings;
using Refit;

namespace Lykke.Service.SellOutEngine.Client.Api
{
    /// <summary>
    /// Provides methods for work with service settings.
    /// </summary>
    [PublicAPI]
    public interface ISettingsApi
    {
        /// <summary>
        /// Returns an account settings that used by service.
        /// </summary>
        /// <returns>The account settings.</returns>
        [Get("/api/settings/account")]
        Task<AccountSettingsModel> GetAccountSettingsAsync();

        /// <summary>
        /// Returns settings of service timers.
        /// </summary>
        /// <returns>The settings of service timers.</returns>
        [Get("/api/settings/timers")]
        Task<TimersSettingsModel> GetTimersSettingsAsync();

        /// <summary>
        /// Saves settings of service timers.
        /// </summary>
        /// <param name="model">The settings of service timers.</param>
        /// <param name="userId">The identifier of the user that execute the operation.</param>
        [Post("/api/settings/timers")]
        Task SaveTimersSettingsAsync([Body] TimersSettingsModel model, string userId);

        /// <summary>
        /// Returns settings of quote timeouts.
        /// </summary>
        /// <returns>The settings of quote timeouts.</returns>
        [Get("/api/settings/quotes")]
        Task<QuoteTimeoutSettingsModel> GetQuoteTimeoutSettingsAsync();

        /// <summary>
        /// Saves settings of quote timeouts.
        /// </summary>
        /// <param name="model">The settings of quote timeouts.</param>
        /// <param name="userId">The identifier of the user that execute the operation.</param>
        [Post("/api/settings/quotes")]
        Task SaveQuoteTimeoutSettingsAsync([Body] QuoteTimeoutSettingsModel model, string userId);
    }
}
