using System;
using JetBrains.Annotations;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.SellOutEngine.AzureRepositories.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class QuoteTimeoutSettingsEntity : AzureTableEntity
    {
        private bool _enabled;
        private TimeSpan _value;

        public QuoteTimeoutSettingsEntity()
        {
        }

        public QuoteTimeoutSettingsEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    MarkValueTypePropertyAsDirty("Enabled");
                }
            }
        }

        public TimeSpan Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    MarkValueTypePropertyAsDirty("Value");
                }
            }
        }
    }
}
