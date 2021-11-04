using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DotNetCoreDecorators.Tests
{


    public class PersistedRepository
    {
        
        private readonly Dictionary<string, string> _items = new Dictionary<string, string>
        {
            ["Id1"] = "Value1",
            ["Id2"] = "Value2",
            ["Id3"] = "Value3",
            ["Id4"] = "Value4",
            ["Id5"] = "Value5",
        };
        
        public string LastAccessedId { get; private set; }

        
        public int AccessCount { get; private set; }
        public Task<string> GetValueAsync(string id)
        {
            LastAccessedId = id;
            AccessCount++;
            
            return _items.ContainsKey(id) 
                ? Task.FromResult(_items[id]) 
                : Task.FromResult<string>(null);
        }
        
    }



    public class TestCachedData
    {

        [Test]
        public void TestGettingCachedData()
        {

            var repo = new PersistedRepository();
            
            var cachedData = new CachedData<string, string>(3)
                .ResolveValue(key => repo.GetValueAsync(key));


            var value = cachedData.GetAsync("Id2").AsTask().Result;
            
            Assert.AreEqual("Id2", repo.LastAccessedId);
            Assert.AreEqual("Value2", value);
            Assert.AreEqual(1, repo.AccessCount);
            
            value = cachedData.GetAsync("Id2").AsTask().Result;
            
            Assert.AreEqual("Id2", repo.LastAccessedId);
            Assert.AreEqual("Value2", value);
            Assert.AreEqual(1, repo.AccessCount);

        }
        
        [Test]
        public void TestGettingCachedDataMoreThenWeCanCache()
        {

            var repo = new PersistedRepository();
            
            var cachedData = new CachedData<string, string>(3)
                .ResolveValue(key => repo.GetValueAsync(key));


            var value = cachedData.GetAsync("Id2").AsTask().Result;
            
            Assert.AreEqual("Id2", repo.LastAccessedId);
            Assert.AreEqual("Value2", value);
            Assert.AreEqual(1, repo.AccessCount);
            
            value = cachedData.GetAsync("Id1").AsTask().Result;
            
            Assert.AreEqual("Id1", repo.LastAccessedId);
            Assert.AreEqual("Value1", value);
            Assert.AreEqual(2, repo.AccessCount);
            
            value = cachedData.GetAsync("Id3").AsTask().Result;
            
            Assert.AreEqual("Id3", repo.LastAccessedId);
            Assert.AreEqual("Value3", value);
            Assert.AreEqual(3, repo.AccessCount);
            
            
            value = cachedData.GetAsync("Id1").AsTask().Result;
            
            Assert.AreEqual("Id3", repo.LastAccessedId);
            Assert.AreEqual("Value1", value);
            Assert.AreEqual(3, repo.AccessCount);
            
            
            value = cachedData.GetAsync("Id4").AsTask().Result;
            
            Assert.AreEqual("Id4", repo.LastAccessedId);
            Assert.AreEqual("Value4", value);
            Assert.AreEqual(4, repo.AccessCount);
            Assert.AreEqual(3, cachedData.CachedCount);

        }
    }
}