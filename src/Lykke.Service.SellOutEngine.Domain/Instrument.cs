namespace Lykke.Service.SellOutEngine.Domain
{
    public class Instrument
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
        
        /// <summary>
        /// Indicates that the instrument was approved after auto creation.
        /// </summary>
        public bool IsApproved { get; set; }

        public void Update(Instrument instrument)
        {
            QuoteSource = instrument.QuoteSource;
            Markup = instrument.Markup;
            Levels = instrument.Levels;
            MinSpread = instrument.MinSpread;
            MaxSpread = instrument.MaxSpread;
            Mode = instrument.Mode;
        }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
