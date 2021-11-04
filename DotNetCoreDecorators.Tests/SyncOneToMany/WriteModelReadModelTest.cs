using System;
using System.Linq;
using NUnit.Framework;

namespace DotNetCoreDecorators.Tests.SyncOneToMany
{
    public class WriteModelReadModelTest
    {
        
        [Test]
        public void TestUpdateOnUpdateServiceReadFromCacheWithMultiData()
        {
            
            var database = new DataBaseWithManyValuesMock();;
            
            var publisherSubscriber = new PublisherSubscriberMock<SyncQueueMultiObjectEvent>();
            
            var accountWriterMock = new AccountsWriterWithMultipleValuesMock(database, publisherSubscriber);
            
            var cache = new CachedDataOneToManyWithSynchronization<string, string, DomainObjectMultiMock>(publisherSubscriber, 10);

            cache.ResolveValue(async id => (await database.GetAsync(id)).ToDictionary(itm => itm.RowKey));

            var domain = new DomainObjectMultiMock
            {
                PartitionKey = "pk",
                RowKey = "rk1",
                Value = "First Value",
                TimeStamp = DateTime.Parse("2019-01-01T00:00:00")
            };
            
            accountWriterMock.Update(domain);

            var valueFromCache = cache.GetAsync("pk").Result["rk1"];
            
            Assert.AreEqual(domain.Value, valueFromCache.Value);
            //First time we read value from database - we do not have it in cache
            Assert.AreEqual(1, database.ReadAttempts);
            
            var domain2 = new DomainObjectMultiMock
            {
                PartitionKey = "pk",
                RowKey = "rk2",
                Value = "First Value RK2",
                TimeStamp = DateTime.Parse("2019-01-01T00:00:00")
            };
            
            accountWriterMock.Update(domain2);

            var valuesFromCache = cache.GetAsync("pk").Result;

            valueFromCache = valuesFromCache["rk1"];
            Assert.AreEqual(domain.Value, valueFromCache.Value);
            
            valueFromCache = valuesFromCache["rk2"];
            Assert.AreEqual(domain2.Value, valueFromCache.Value);
            
            //Second time we should not try to read value from database - we already had it in database. We just update it;
            Assert.AreEqual(1, database.ReadAttempts);

        }
        
    }
}