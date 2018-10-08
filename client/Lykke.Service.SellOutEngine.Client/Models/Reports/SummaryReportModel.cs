using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Client.Models.Reports
{
    /// <summary>
    /// Represents a summary report by asset pair. 
    /// </summary>
    [PublicAPI]
    public class SummaryReportModel
    {
        /// <summary>
        /// The identifier of the asset pair.
        /// </summary>
        public string AssetPairId { get; set; }

        /// <summary>
        /// The minimal price of trades.
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// The maximal price of trades.
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// The average price of trades.
        /// </summary>
        public decimal AvgPrice { get; set; }

        /// <summary>
        /// The total volume of the base asset that was sold.
        /// </summary>
        public decimal TotalSellBaseAssetVolume { get; set; }

        /// <summary>
        /// The total volume of the quote asset that was bought.
        /// </summary>
        public decimal TotalBuyQuoteAssetVolume { get; set; }

        /// <summary>
        /// The number of sell trades.
        /// </summary>
        public int SellTradesCount { get; set; }
    }
}
