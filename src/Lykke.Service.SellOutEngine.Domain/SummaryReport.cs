using System;

namespace Lykke.Service.SellOutEngine.Domain
{
    /// <summary>
    /// Represents a summary report by asset pair.
    /// </summary>
    public class SummaryReport
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

        public void ApplyTrade(Trade trade)
        {
            if (MinPrice == decimal.Zero)
                MinPrice = trade.Price;
            else
                MinPrice = Math.Min(MinPrice, trade.Price);

            MaxPrice = Math.Max(MaxPrice, trade.Price);

            AvgPrice = (MinPrice + MaxPrice) / 2;

            TotalSellBaseAssetVolume += trade.Volume;

            TotalBuyQuoteAssetVolume += trade.OppositeVolume;

            SellTradesCount++;
        }
    }
}
