using System;

namespace Lykke.Service.SellOutEngine.Domain
{
    public class Quote
    {
        public Quote(string assetPairId, DateTime time, decimal ask, decimal bid, string source)
        {
            AssetPairId = assetPairId;
            Time = time;
            Ask = ask;
            Bid = bid;
            Mid = (ask + bid) / 2m;
            Spread = Ask - Bid;
            Source = source;
        }

        public string AssetPairId { get; }

        public DateTime Time { get; }

        public decimal Ask { get; }

        public decimal Bid { get; }

        public decimal Mid { get; }

        public decimal Spread { get; }

        public string Source { get; }
    }
}
