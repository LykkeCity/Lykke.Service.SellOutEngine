using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Client.Models.Instruments
{
    /// <summary>
    /// Represent an instrument that used by trading algorithm. 
    /// </summary>
    [PublicAPI]
    public class InstrumentModel
    {
        /// <summary>
        /// The identifier of the asset pair.
        /// </summary>
        public string AssetPairId { get; set; }

        /// <summary>
        /// The name of the adapter that used as source of quotes.
        /// </summary>
        public string QuoteSource { get; set; }

        /// <summary>
        /// The risk markup.
        /// </summary>
        public decimal Markup { get; set; }

        /// <summary>
        /// The number of limit orders in order book.
        /// </summary>
        public int Levels { get; set; }

        /// <summary>
        /// The minimal spread that should be applied to the order book.
        /// </summary>
        public decimal MinSpread { get; set; }

        /// <summary>
        /// The maximal spread that should be applied to the order book.
        /// </summary>
        public decimal MaxSpread { get; set; }

        /// <summary>
        /// The mode of the instrument.
        /// </summary>
        public InstrumentMode Mode { get; set; }
    }
}
