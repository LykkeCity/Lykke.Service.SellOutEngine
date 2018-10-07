using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.SellOutEngine.Client.Models.Trades
{
    /// <summary>
    /// Represents a trade.
    /// </summary>
    [PublicAPI]
    public class TradeModel
    {
        /// <summary>
        /// The identifier of the trade.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The internal identifier of the limit order which executed while trade.
        /// </summary>
        public string LimitOrderId { get; set; }

        /// <summary>
        /// The exchange identifier of the limit order which executed while trade.
        /// </summary>
        public string ExchangeOrderId { get; set; }

        /// <summary>
        /// The asset pair.
        /// </summary>
        public string AssetPairId { get; set; }

        /// <summary>
        /// The type of the trade.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TradeType Type { get; set; }

        /// <summary>
        /// The time of the trade.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The price of the trade.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The volume of the trade.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// The remaining volume of the trade.
        /// </summary>
        public decimal RemainingVolume { get; set; }

        /// <summary>
        /// The opposite volume of the trade.
        /// </summary>
        public decimal OppositeSideVolume { get; set; }

        /// <summary>
        /// The identifier of the opposite client.
        /// </summary>
        public string OppositeClientId { get; set; }

        /// <summary>
        /// The identifier of the opposite limit order.
        /// </summary>
        public string OppositeLimitOrderId { get; set; }
    }
}
