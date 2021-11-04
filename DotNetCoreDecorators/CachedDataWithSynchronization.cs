using System;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{

    public interface ISyncCacheMessage<out TKey, out TValue>
    {
        TKey Key { get; }
        TValue Value { get; }
    }
    
    public class CachedDataWithSynchronization<TKey, TValue> where TValue: IDomainObjectTimeStamp
    {

        private readonly CachedData<TKey, TValue> _cachedData;

        public CachedDataWithSynchronization(ISubscriber<ISyncCacheMessage<TKey, TValue>> keyChangesSubscriber, int maxCapacity)
        {
            _cachedData = new CachedData<TKey, TValue>(maxCapacity);
            keyChangesSubscriber.Subscribe(ItemChangedAsync);
        }


        private ValueTask ItemChangedAsync(ISyncCacheMessage<TKey, TValue> newMessage)
        {
            _cachedData.UpdateValueInCacheIfExists(newMessage.Key, 
                itmInCache => newMessage.Value.TimeStamp > itmInCache.TimeStamp ? newMessage.Value : default);
            return new ValueTask();
        }

        public CachedDataWithSynchronization<TKey, TValue> ResolveValue(Func<TKey, Task<TValue>> getValue)
        {
            _cachedData.ResolveValue(getValue);
            return this;
        }

        public ValueTask<TValue> GetAsync(TKey key)
        {
            return _cachedData.GetAsync(key);
        }
        
    }
}