using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{
    
    public interface ISyncCacheMessage<TPartitionKey, TRowKey, TValue> 
    {
        TPartitionKey PartitionKey { get; }
        TRowKey RowKey { get; }
        TValue Value { get; }
    }
    
    public class CachedDataOneToManyWithSynchronization<TPartitionKey, TRowKey, TValue> where TValue: IDomainObjectTimeStamp
    {

        private readonly CachedData<TPartitionKey, IReadOnlyDictionary<TRowKey, TValue>> _cachedData;

        public CachedDataOneToManyWithSynchronization(ISubscriber<ISyncCacheMessage<TPartitionKey, TRowKey, TValue>> keyChangesSubscriber, int maxCapacity)
        {
            _cachedData = new CachedData<TPartitionKey, IReadOnlyDictionary<TRowKey, TValue>>(maxCapacity);
            keyChangesSubscriber.Subscribe(ItemChangedAsync);
        }

        private ValueTask ItemChangedAsync(ISyncCacheMessage<TPartitionKey, TRowKey, TValue> newMessage)
        {
            _cachedData.UpdateValueInCacheIfExists(newMessage.PartitionKey,
                itemsInCache =>
                {
                    if (!itemsInCache.ContainsKey(newMessage.RowKey))
                        return new Dictionary<TRowKey, TValue>(itemsInCache)
                        {
                            {newMessage.RowKey, newMessage.Value}
                        };

                    var itemToUpdate = itemsInCache[newMessage.RowKey];

                    return newMessage.Value.TimeStamp > itemToUpdate.TimeStamp
                        ? new Dictionary<TRowKey, TValue>(itemsInCache) {[newMessage.RowKey] = newMessage.Value}
                        : null;
                });

            return new ValueTask();
        }

        public CachedDataOneToManyWithSynchronization<TPartitionKey, TRowKey, TValue> 
            ResolveValue(Func<TPartitionKey, Task<IReadOnlyDictionary<TRowKey, TValue>>> getValue)
        {
            _cachedData.ResolveValue(getValue);
            return this;
        }

        public ValueTask<IReadOnlyDictionary<TRowKey, TValue>> GetAsync(TPartitionKey key)
        {
            return _cachedData.GetAsync(key);
        }
        
    }
}