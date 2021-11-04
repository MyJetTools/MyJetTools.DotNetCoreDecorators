namespace DotNetCoreDecorators.Tests.SyncOneToOne
{
    public class SyncQueueEvent : ISyncCacheMessage<string, AccountMock>
    {
        public SyncQueueEvent(string key, AccountMock value)
        {
            Key = key;
            Value = value;
        }
        
        public string Key { get; }
        public AccountMock Value { get; }
    }
}