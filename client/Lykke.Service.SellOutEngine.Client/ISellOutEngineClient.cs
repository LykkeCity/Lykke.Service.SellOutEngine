using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Api;

namespace Lykke.Service.SellOutEngine.Client
{
    /// <summary>
    /// Sell out engine service client.
    /// </summary>
    [PublicAPI]
    public interface ISellOutEngineClient
    {
        /// <summary>
        /// Balances API.
        /// </summary>
        IBalancesApi Balances { get; }

        /// <summary>
        /// Instruments API.
        /// </summary>
        IInstrumentsApi Instruments { get; }

        /// <summary>
        /// Order books API.
        /// </summary>
        IOrderBooksApi OrderBooks { get; }

        /// <summary>
        /// Reports API.
        /// </summary>
        IReportsApi Reports { get; }

        /// <summary>
        /// Settings API.
        /// </summary>
        ISettingsApi Settings { get; }

        /// <summary>
        /// Trades API.
        /// </summary>
        ITradesApi Trades { get; }
    }
}
