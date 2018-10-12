using System;

namespace Lykke.Service.SellOutEngine.Domain
{
    /// <summary>
    /// Represents a settings of quote timeouts.
    /// </summary>
    public class QuoteTimeoutSettings
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
