using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators.Tests.SyncOneToMany
{
    public class DomainObjectMultiMock : IDomainObjectTimeStamp
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
    }
    
    public class DataBaseWithManyValuesMock
    {
        private readonly Dictionary<string, Dictionary<string, DomainObjectMultiMock>> _items = new Dictionary<string, Dictionary<string, DomainObjectMultiMock>>();

        public void Update(DomainObjectMultiMock itm)
        {
            if (!_items.ContainsKey(itm.PartitionKey))
                _items.Add(itm.PartitionKey, new Dictionary<string, DomainObjectMultiMock>());

            var dict = _items[itm.PartitionKey];

            if (dict.ContainsKey(itm.RowKey))
                dict[itm.RowKey] = itm;
            else
                dict.Add(itm.RowKey, itm);
        }

        public int ReadAttempts { get; private set; }
        public Task<IEnumerable<DomainObjectMultiMock>> GetAsync(string partitionKey)
        {

            ReadAttempts++;
            
            if (_items.ContainsKey(partitionKey))
                return Task.FromResult<IEnumerable<DomainObjectMultiMock>>(_items[partitionKey].Values);

            return Task.FromResult<IEnumerable<DomainObjectMultiMock>>(Array.Empty<DomainObjectMultiMock>());
        }
    }
}