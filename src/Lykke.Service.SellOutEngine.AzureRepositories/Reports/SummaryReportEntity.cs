using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.SellOutEngine.AzureRepositories.Reports
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class SummaryReportEntity : AzureTableEntity
    {
        private decimal _minPrice;
        private decimal _maxPrice;
        private decimal _avgPrice;
        private decimal _totalSellBaseAssetVolume;
        private decimal _totalBuyQuoteAssetVolume;
        private int _sellTradesCount;

        public SummaryReportEntity()
        {
        }

        public SummaryReportEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string AssetPairId { get; set; }

        public decimal MinPrice
        {
            get => _minPrice;
            set
            {
                if (_minPrice != value)
                {
                    _minPrice = value;
                    MarkValueTypePropertyAsDirty("MinPrice");
                }
            }
        }

        public decimal MaxPrice
        {
            get => _maxPrice;
            set
            {
                if (_maxPrice != value)
                {
                    _maxPrice = value;
                    MarkValueTypePropertyAsDirty("MaxPrice");
                }
            }
        }

        public decimal AvgPrice
        {
            get => _avgPrice;
            set
            {
                if (_avgPrice != value)
                {
                    _avgPrice = value;
                    MarkValueTypePropertyAsDirty("AvgPrice");
                }
            }
        }

        public decimal TotalSellBaseAssetVolume
        {
            get => _totalSellBaseAssetVolume;
            set
            {
                if (_totalSellBaseAssetVolume != value)
                {
                    _totalSellBaseAssetVolume = value;
                    MarkValueTypePropertyAsDirty("TotalSellBaseAssetVolume");
                }
            }
        }

        public decimal TotalBuyQuoteAssetVolume
        {
            get => _totalBuyQuoteAssetVolume;
            set
            {
                if (_totalBuyQuoteAssetVolume != value)
                {
                    _totalBuyQuoteAssetVolume = value;
                    MarkValueTypePropertyAsDirty("TotalBuyQuoteAssetVolume");
                }
            }
        }

        public int SellTradesCount
        {
            get => _sellTradesCount;
            set
            {
                if (_sellTradesCount != value)
                {
                    _sellTradesCount = value;
                    MarkValueTypePropertyAsDirty("SellTradesCount");
                }
            }
        }
    }
}
