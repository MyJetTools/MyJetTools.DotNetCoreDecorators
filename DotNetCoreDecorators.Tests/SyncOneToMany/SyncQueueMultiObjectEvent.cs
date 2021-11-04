
namespace DotNetCoreDecorators.Tests.SyncOneToMany
{

    public class SyncQueueMultiObjectEvent : ISyncCacheMessage<string, string, DomainObjectMultiMock>
    {
        public SyncQueueMultiObjectEvent(string partitionKey, string rowKey, DomainObjectMultiMock value)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Value = value;
        }
        
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DomainObjectMultiMock Value { get; set; }
    }
}