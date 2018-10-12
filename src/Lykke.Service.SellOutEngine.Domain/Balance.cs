namespace Lykke.Service.SellOutEngine.Domain
{
    /// <summary>
    /// Represents an asset balance. 
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Balance"/> of asset with amount of balance.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="amount">The amount of balance.</param>
        /// <param name="reserved">The amount that currently are reserved.</param>
        public Balance(string assetId, decimal amount, decimal reserved)
        {
            AssetId = assetId;
            Amount = amount;
            Reserved = reserved;
        }

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
