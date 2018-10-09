using System;
using System.Collections.Generic;
using Common;
using Lykke.Service.SellOutEngine.Domain;

namespace Lykke.Service.SellOutEngine.DomainServices.Traders
{
    public static class EvenlyDistributedVolumeTrader
    {
        public static IReadOnlyCollection<LimitOrder> Calculate(
            Quote quote,
            Instrument instrument,
            decimal volume,
            int priceAccuracy,
            int volumeAccuracy)
        {
            var limitOrders = new List<LimitOrder>();

            if (instrument.Levels < 1)
                return limitOrders;

            decimal coefficient = (quote.Ask - quote.Bid) / quote.Mid;

            decimal minSellPrice = quote.Mid + quote.Mid * ((instrument.MinSpread + coefficient) / 2);

            decimal maxSellPrice = quote.Mid + quote.Mid * ((instrument.MaxSpread + coefficient) / 2);

            decimal priceStep = instrument.Levels > 1
                ? (maxSellPrice - minSellPrice) / (instrument.Levels - 1)
                : 0;

            decimal limitOrderVolume = volume / instrument.Levels;

            for (int i = 1; i <= instrument.Levels; i++)
            {
                decimal delta = priceStep * (i - 1);

                limitOrders.Add(new LimitOrder(
                    ((minSellPrice + delta) * (1 + instrument.Markup)).TruncateDecimalPlaces(priceAccuracy, true),
                    Math.Round(limitOrderVolume, volumeAccuracy),
                    LimitOrderType.Sell));
            }

            return limitOrders;
        }
    }
}
