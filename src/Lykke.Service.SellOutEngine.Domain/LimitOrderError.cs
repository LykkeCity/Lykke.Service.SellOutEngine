namespace Lykke.Service.SellOutEngine.Domain
{
    /// <summary>
    /// Specifies limit order error.
    /// </summary>
    public enum LimitOrderError
    {
        /// <summary>
        /// Unspecified error.
        /// </summary>
        None,

        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown,

        /// <summary>
        /// Indicates that an instrument is not allowed to create limit orders on exchange.
        /// </summary>
        Idle,

        /// <summary>
        /// Indicates that the current quota is exceeded allowed timeout.
        /// </summary>
        InvalidQuote,

        /// <summary>
        /// Indicates that the current balance is too low.
        /// </summary>
        LowBalance,

        /// <summary>
        /// Indicates that the current order book has not liquidity.
        /// </summary>
        NoLiquidity,

        /// <summary>
        /// Indicates that there are no funds to create limit order.
        /// </summary>
        NotEnoughFunds,

        /// <summary>
        /// Indicates that the opposite volume is greater than quote asset balance.
        /// </summary>
        ReservedVolumeHigherThanBalance,

        /// <summary>
        /// Indicates that the current balance is lower than the reserved amount.
        /// </summary>
        BalanceLowerThanReserved,

        /// <summary>
        /// Indicates that the limit order leads to negative spread.
        /// </summary>
        LeadToNegativeSpread,

        /// <summary>
        /// Indicates that the limit order volume is less than allowed min volume.
        /// </summary>
        TooSmallVolume,

        /// <summary>
        /// Indicates that the limit order price is invalid (less than zero or etc.).
        /// </summary>
        InvalidPrice
    }
}
