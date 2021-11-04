
namespace DotNetCoreDecorators.Tests.SyncOneToMany
{
    
    public class AccountsWriterWithMultipleValuesMock
    {
        private readonly DataBaseWithManyValuesMock _dataBaseMock;
        private readonly IPublisher<SyncQueueMultiObjectEvent> _publisher;

        public AccountsWriterWithMultipleValuesMock(DataBaseWithManyValuesMock dataBaseMock,  IPublisher<SyncQueueMultiObjectEvent> publisher)
        {
            _dataBaseMock = dataBaseMock;
            _publisher = publisher;
        }


        public void Update(DomainObjectMultiMock domainObject)
        {
            _dataBaseMock.Update( domainObject);
            _publisher.PublishAsync(new SyncQueueMultiObjectEvent(domainObject.PartitionKey, domainObject.RowKey, domainObject));
        }
        
    }
}