using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Common;
using Lykke.Service.SellOutEngine.Domain;
using Lykke.Service.SellOutEngine.Domain.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.SellOutEngine.AzureRepositories.Trades
{
    public class TradeRepository : ITradeRepository
    {
        private readonly INoSQLTableStorage<TradeEntity> _storage;
        private readonly INoSQLTableStorage<AzureIndex> _indicesStorage;

        public TradeRepository(
            INoSQLTableStorage<TradeEntity> storage,
            INoSQLTableStorage<AzureIndex> indicesStorage)
        {
            _storage = storage;
            _indicesStorage = indicesStorage;
        }

        public async Task<IReadOnlyCollection<Trade>> GetAsync(DateTime startDate, DateTime endDate, int limit)
        {
            string filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(nameof(TradeEntity.RowKey), QueryComparisons.GreaterThan,
                    GetPartitionKey(endDate.Date.AddDays(1))),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(nameof(TradeEntity.RowKey), QueryComparisons.LessThan,
                    GetPartitionKey(startDate.Date.AddMilliseconds(-1))));

            var query = new TableQuery<TradeEntity>().Where(filter).Take(limit);

            IEnumerable<TradeEntity> entities = await _storage.WhereAsync(query);

            return Mapper.Map<List<Trade>>(entities);
        }

        public async Task<Trade> GetByIdAsync(string tradeId)
        {
            AzureIndex index = await _indicesStorage.GetDataAsync(GetIndexPartitionKey(tradeId),
                GetIndexRowKey(tradeId));

            if (index == null)
                return null;

            TradeEntity entity = await _storage.GetDataAsync(index);

            return Mapper.Map<Trade>(entity);
        }

        public async Task InsertAsync(Trade trade)
        {
            var entity = new TradeEntity(GetPartitionKey(trade.Time), GetRowKey(trade.Id));

            Mapper.Map(trade, entity);

            await _storage.InsertAsync(entity);

            AzureIndex index = new AzureIndex(GetIndexPartitionKey(trade.Id), GetRowKey(trade.Id),
                entity);

            await _indicesStorage.InsertAsync(index);
        }

        private static string GetPartitionKey(DateTime time)
            => (DateTime.MaxValue.Ticks - time.Date.Ticks).ToString("D19");

        private static string GetRowKey(string tradeId)
            => tradeId;

        private static string GetIndexPartitionKey(string tradeId)
            => tradeId.CalculateHexHash32(4);

        private static string GetIndexRowKey(string tradeId)
            => tradeId;
    }
}
