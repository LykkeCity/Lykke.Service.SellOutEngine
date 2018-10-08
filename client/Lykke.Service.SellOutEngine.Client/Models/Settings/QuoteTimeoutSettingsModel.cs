using System;
using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Client.Models.Settings
{
    /// <summary>
    /// Represents a settings of quote timeouts.
    /// </summary>
    [PublicAPI]
    public class QuoteTimeoutSettingsModel
    {
        /// <summary>
        /// Indicates that the timeout is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The value of the timeout.
        /// </summary>
        public TimeSpan Value { get; set; }
    }
}
