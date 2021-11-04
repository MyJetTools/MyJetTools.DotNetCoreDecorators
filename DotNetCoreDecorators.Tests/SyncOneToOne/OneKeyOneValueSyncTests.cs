using System;
using NUnit.Framework;

namespace DotNetCoreDecorators.Tests.SyncOneToOne
{
    public class OneKeyOneValueSyncTests
    {
        
        [Test]
        public void TestUpdateOnUpdateServiceReadFromCache()
        {
            
            var database = new DataBaseMock();
            
            var publisherSubscriber = new PublisherSubscriberMock<SyncQueueEvent>();
            
            var accountWriterMock = new AccountsWriterMock(database, publisherSubscriber);
            
            var cache = new CachedDataWithSynchronization<string, AccountMock>(publisherSubscriber, 10);

            cache.ResolveValue(id => database.GetAsync(id));

            var domain = new AccountMock
            {
                SomeField = "First Value",
                TimeStamp = DateTime.Parse("2019-01-01T00:00:00")
            };
            
            accountWriterMock.Update("Id", domain);

            var valueFromCache = cache.GetAsync("Id").Result;
            
            Assert.AreEqual(domain.SomeField, valueFromCache.SomeField);
            Assert.AreEqual(domain.TimeStamp, valueFromCache.TimeStamp);
            //First time we read value from database - we do not have it in cache
            Assert.AreEqual(1, database.ReadAttempts);
            
            domain = new AccountMock
            {
                SomeField = "Second Value",
                TimeStamp = DateTime.Parse("2019-01-01T00:00:01")
            };
            
            accountWriterMock.Update("Id", domain);

            valueFromCache = cache.GetAsync("Id").Result;
            
            Assert.AreEqual(domain.SomeField, valueFromCache.SomeField);
            Assert.AreEqual(domain.TimeStamp, valueFromCache.TimeStamp);
            
            //Second time we should not try to read value from database - we already had it in database. We just update it;
            Assert.AreEqual(1, database.ReadAttempts);
        }
    }
}