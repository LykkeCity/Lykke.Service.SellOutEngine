using Lykke.HttpClientGenerator;
using Lykke.Service.SellOutEngine.Client.Api;

namespace Lykke.Service.SellOutEngine.Client
{
    /// <summary>
    /// Sell out engine service client.
    /// </summary>
    public class SellOutEngineClient : ISellOutEngineClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SellOutEngineClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary> 
        public SellOutEngineClient(IHttpClientGenerator httpClientGenerator)
        {
            Balances = httpClientGenerator.Generate<IBalancesApi>();
            Instruments = httpClientGenerator.Generate<IInstrumentsApi>();
            OrderBooks = httpClientGenerator.Generate<IOrderBooksApi>();
            Reports = httpClientGenerator.Generate<IReportsApi>();
            Settings = httpClientGenerator.Generate<ISettingsApi>();
            Trades = httpClientGenerator.Generate<ITradesApi>();
        }

        /// <inheritdoc/>
        public IBalancesApi Balances { get; }

        /// <inheritdoc/>
        public IInstrumentsApi Instruments { get; }

        /// <inheritdoc/>
        public IOrderBooksApi OrderBooks { get; }

        /// <inheritdoc/>
        public IReportsApi Reports { get; }

        /// <inheritdoc/>
        public ISettingsApi Settings { get; }

        /// <inheritdoc/>
        public ITradesApi Trades { get; }
    }
}
