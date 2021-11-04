using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators
{
    public class CachedData<TKey, TValue>
    {
        private readonly int _maxCapacity;

        private long _nextOrderId;

        public class CachedItem
        {
            public CachedItem(TKey key, TValue item, long nexOrderId)
            {
                Key = key;
                Value = item;
                UpdateLastAccess(nexOrderId);
            }


            internal void UpdateLastAccess(long nextOrderId)
            {
                LastAccessed = DateTime.UtcNow;
                OrderId = nextOrderId;
            }

            public TKey Key { get; }
            public TValue Value { get; }

            public long OrderId { get; set; }
            
            public long QueueId { get; set; }

            public DateTime LastAccessed { get; private set; }

        }


        private Func<TKey, Task<TValue>> _getValue;

        public CachedData(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }


        private readonly object _lockObject = new object();

        private readonly Dictionary<TKey, CachedItem> _cachedItems = new Dictionary<TKey, CachedItem>();

        private readonly SortedList<long, CachedItem> _itemsSortedByAccess = new SortedList<long, CachedItem>();




        private void RemoveIfTooMuch()
        {
            while (_cachedItems.Count >= _maxCapacity)
            {
                var itemToRemove = _itemsSortedByAccess.Values[0];
                _cachedItems.Remove(itemToRemove.Key);
                _itemsSortedByAccess.Remove(itemToRemove.OrderId);
            }
        }

        private void RemoveFromCacheIfExists(TKey key)
        {
            if (!_cachedItems.ContainsKey(key))
                return;
            
            var item = _cachedItems[key];
            _cachedItems.Remove(key);
            _itemsSortedByAccess.Remove(item.OrderId);
        }

        private async Task<TValue> GetDataFromExternalSource(TKey key)
        {
            var value = await _getValue(key);

            if (value == null)
                return default;

       
            UpdateValueInCache(key, value);

            return value;

        }

        private void UpdateValueInCache(TKey key, TValue value)
        {
            lock (_lockObject)
            {
                RemoveFromCacheIfExists(key);
                RemoveIfTooMuch();

                var item = new CachedItem(key, value, _nextOrderId++);

                _cachedItems.Add(key, item);
                _itemsSortedByAccess.Add(item.OrderId, item);
            }
        } 

        public void RemoveIfExists(TKey key)
        {
            lock (_lockObject)
                RemoveFromCacheIfExists(key);
        }


        public async Task UpdateValueInCacheIfExistsAsync(TKey key)
        {

            lock (_lockObject)
            {
                if (!_cachedItems.ContainsKey(key))
                    return;
            }

            await GetDataFromExternalSource(key);
        }


        private CachedItem GetValueOrNull(TKey key)
        {
            lock (_lockObject)
            {
                if (!_cachedItems.ContainsKey(key))
                    return null;

                return _cachedItems[key];
            }
            
        }


        public void UpdateValueInCacheIfExists(TKey key, Func<TValue, TValue> updateValueCallback)
        {

            var valueInCache = GetValueOrNull(key);
            if (valueInCache == null)
                return;


            var newValue = updateValueCallback(valueInCache.Value);

            if (newValue != null)
                UpdateValueInCache(key, newValue);
        }


        public ValueTask<TValue> GetAsync(TKey key)
        {

            if (_getValue == null)
                throw new Exception("Value resolver is not set");

            lock (_lockObject)
            {
                if (_cachedItems.ContainsKey(key))
                {
                    var item = _cachedItems[key];
                    _itemsSortedByAccess.Remove(item.OrderId);
                    item.UpdateLastAccess(_nextOrderId++);
                    _itemsSortedByAccess.Add(item.OrderId, item);
                    return new ValueTask<TValue>(item.Value);
                }
            }

            return new ValueTask<TValue>(GetDataFromExternalSource(key));
        }

        public CachedData<TKey, TValue> ResolveValue(Func<TKey, Task<TValue>> getValue)
        {
            _getValue = getValue;
            return this;
        }

        public int CachedCount
        {
            get
            {
                lock (_lockObject)
                    return _cachedItems.Count;
            }
        }

    }
}