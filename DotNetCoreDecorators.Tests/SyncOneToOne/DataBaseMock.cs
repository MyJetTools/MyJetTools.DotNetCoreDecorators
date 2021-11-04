using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreDecorators.Tests.SyncOneToOne
{
    public class AccountMock : IDomainObjectTimeStamp
    {
        public string SomeField { get; set; }
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    
    
    public class DataBaseMock
    {
        
        private readonly Dictionary<string, AccountMock> _items = new Dictionary<string, AccountMock>();

        public void Update(string id, AccountMock itm)
        {
            if (_items.ContainsKey(id))
                _items[id] = itm;
            else
                _items.Add(id, itm);
        }
        
        public int ReadAttempts { get; private set; }
        
        public Task<AccountMock> GetAsync(string id)
        {
            ReadAttempts++;
            var result = _items.ContainsKey(id) ? _items[id] : null;
            return Task.FromResult(result);
        }
    }
}