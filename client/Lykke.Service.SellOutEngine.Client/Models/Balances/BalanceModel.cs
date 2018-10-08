using JetBrains.Annotations;

namespace Lykke.Service.SellOutEngine.Client.Models.Balances
{
    /// <summary>
    /// Represents an asset balance. 
    /// </summary>
    [PublicAPI]
    public class BalanceModel
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// The current amount of balance.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The amount that currently are reserved.
        /// </summary>
        public decimal Reserved { get; set; }
    }
}
