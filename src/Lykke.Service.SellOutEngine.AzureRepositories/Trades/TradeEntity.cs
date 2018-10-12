using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.SellOutEngine.Domain;

namespace Lykke.Service.SellOutEngine.AzureRepositories.Trades
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class TradeEntity : AzureTableEntity
    {
        private TradeType _type;
        private DateTime _time;
        private decimal _price;
        private decimal _volume;
        private decimal _remainingVolume;
        private decimal _oppositeVolume;

        public TradeEntity()
        {
        }

        public TradeEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string Id { get; set; }

        public string LimitOrderId { get; set; }

        public string ExchangeOrderId { get; set; }

        public string AssetPairId { get; set; }

        public TradeType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    MarkValueTypePropertyAsDirty("Type");
                }
            }
        }

        public DateTime Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    MarkValueTypePropertyAsDirty("Time");
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    MarkValueTypePropertyAsDirty("Price");
                }
            }
        }

        public decimal Volume
        {
            get => _volume;
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    MarkValueTypePropertyAsDirty("Volume");
                }
            }
        }

        public decimal RemainingVolume
        {
            get => _remainingVolume;
            set
            {
                if (_remainingVolume != value)
                {
                    _remainingVolume = value;
                    MarkValueTypePropertyAsDirty("RemainingVolume");
                }
            }
        }

        public decimal OppositeVolume
        {
            get => _oppositeVolume;
            set
            {
                if (_oppositeVolume != value)
                {
                    _oppositeVolume = value;
                    MarkValueTypePropertyAsDirty("OppositeVolume");
                }
            }
        }

        public string OppositeClientId { get; set; }

        public string OppositeLimitOrderId { get; set; }
    }
}
