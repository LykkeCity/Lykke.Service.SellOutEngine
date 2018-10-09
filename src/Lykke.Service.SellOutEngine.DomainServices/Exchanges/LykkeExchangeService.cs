using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.MatchingEngine.Connector.Abstractions.Services;
using Lykke.MatchingEngine.Connector.Models.Api;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Extensions;
using Lykke.Service.SellOutEngine.Domain.Services;
using Lykke.Service.SellOutEngine.DomainServices.Extensions;

namespace Lykke.Service.SellOutEngine.DomainServices.Exchanges
{
    [UsedImplicitly]
    public class LykkeExchangeService : ILykkeExchangeService
    {
        private readonly IMatchingEngineClient _matchingEngineClient;
        private readonly ISettingsService _settingsService;
        private readonly ILog _log;

        public LykkeExchangeService(
            IMatchingEngineClient matchingEngineClient,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _matchingEngineClient = matchingEngineClient;
            _settingsService = settingsService;
            _log = logFactory.CreateLog(this);
        }

        public async Task ApplyAsync(string assetPairId, IReadOnlyCollection<LimitOrder> limitOrders)
        {
            string walletId = await _settingsService.GetWalletIdAsync();

            if (string.IsNullOrEmpty(walletId))
                throw new Exception("WalletId is not set");

            var map = new Dictionary<string, string>();

            var multiOrderItems = new List<MultiOrderItemModel>();

            foreach (LimitOrder limitOrder in limitOrders)
            {
                var multiOrderItem = new MultiOrderItemModel
                {
                    Id = Guid.NewGuid().ToString("D"),
                    OrderAction = limitOrder.Type.ToOrderAction(),
                    Price = (double) limitOrder.Price,
                    Volume = (double) Math.Abs(limitOrder.Volume)
                };

                multiOrderItems.Add(multiOrderItem);

                map[multiOrderItem.Id] = limitOrder.Id;
            }

            var multiLimitOrder = new MultiLimitOrderModel
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = walletId,
                AssetPairId = assetPairId,
                CancelPreviousOrders = true,
                Orders = multiOrderItems,
                CancelMode = CancelMode.BothSides
            };

            _log.InfoWithDetails("ME place multi limit order request", multiLimitOrder);

            MultiLimitOrderResponse response;

            try
            {
                response = await _matchingEngineClient.PlaceMultiLimitOrderAsync(multiLimitOrder);
            }
            catch (Exception exception)
            {
                _log.ErrorWithDetails(exception, "An error occurred during creating limit orders", multiLimitOrder);

                throw;
            }

            if (response == null)
                throw new Exception("ME response is null");

            foreach (LimitOrderStatusModel orderStatus in response.Statuses)
            {
                if (map.TryGetValue(orderStatus.Id, out var limitOrderId))
                {
                    var limitOrder = limitOrders.Single(e => e.Id == limitOrderId);

                    limitOrder.Error = orderStatus.Status.ToOrderError();
                    limitOrder.ErrorMessage = limitOrder.Error != LimitOrderError.Unknown
                        ? orderStatus.StatusReason
                        : !string.IsNullOrEmpty(orderStatus.StatusReason)
                            ? orderStatus.StatusReason
                            : "Unknown error";
                }
                else
                {
                    _log.Warning("ME returned status for order which is not in the request",
                        context: $"order: {orderStatus.Id}");
                }
            }

            string[] ignoredOrdersByMe = response.Statuses
                .Select(x => x.Id)
                .Except(multiLimitOrder.Orders.Select(x => x.Id))
                .ToArray();

            if (ignoredOrdersByMe.Any())
            {
                _log.WarningWithDetails("ME didn't return status for orders",
                    $"pair: {assetPairId}, orders: {string.Join(", ", ignoredOrdersByMe)}");
            }

            _log.InfoWithDetails("ME place multi limit order response", response);
        }
    }
}
