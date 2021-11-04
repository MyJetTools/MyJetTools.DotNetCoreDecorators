namespace DotNetCoreDecorators.Tests.SyncOneToOne
{
    public class AccountsWriterMock
    {
        private readonly DataBaseMock _dataBaseMock;
        private readonly IPublisher<SyncQueueEvent> _publisher;

        public AccountsWriterMock(DataBaseMock dataBaseMock,  IPublisher<SyncQueueEvent> publisher)
        {
            _dataBaseMock = dataBaseMock;
            _publisher = publisher;
        }


        public void Update(string key, AccountMock domainObject)
        {
            _dataBaseMock.Update(key, domainObject);
            _publisher.PublishAsync(new SyncQueueEvent(key, domainObject));
        }
        
    }
}