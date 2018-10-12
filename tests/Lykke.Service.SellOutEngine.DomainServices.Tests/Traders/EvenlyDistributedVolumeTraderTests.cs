using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.DomainServices.Traders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.SellOutEngine.DomainServices.Tests.Traders
{
    [TestClass]
    public class EvenlyDistributedVolumeTraderTests
    {
        [TestMethod]
        public void Calculate_Limit_Orders()
        {
            // arrange

            var quote = new Quote("BTCUSD", DateTime.UtcNow, 6000, 5000, "lykke");

            var instrument = new Instrument
            {
                AssetPairId = "BTCUSD",
                QuoteSource = "lykke",
                Mode = InstrumentMode.Active,
                Levels = 3,
                Markup = .1m,
                MinSpread = .01m,
                MaxSpread = .06m
            };

            decimal volume = 10;

            var expectedLimitOrders = new[]
            {
                new LimitOrder(6630.250m, 3.33333333m, LimitOrderType.Sell),
                new LimitOrder(6705.875m, 3.33333333m, LimitOrderType.Sell),
                new LimitOrder(6781.500m, 3.33333333m, LimitOrderType.Sell)
            };

            // act

            IReadOnlyCollection<LimitOrder> actualLimitOrders =
                EvenlyDistributedVolumeTrader.Calculate(quote, instrument, volume, 3, 8);

            // assert

            Assert.IsTrue(AreEqual(expectedLimitOrders, actualLimitOrders));
        }

        private bool AreEqual(IReadOnlyCollection<LimitOrder> a, IReadOnlyCollection<LimitOrder> b)
        {
            if (a.Count != b.Count)
                return false;

            return a.All(o => b.Any(p => p.Type == o.Type && p.Volume == o.Volume && p.Price == o.Price));
        }
    }
}
