using System.Linq;
using NUnit.Framework;

namespace DotNetCoreDecorators.Tests
{
    public class TestReplaceIfExists
    {
        [Test]
        public void TestReplaceIfExistsOnEnumerable()
        {
            var elements = new[] {1, 2, 3, 4, 5};

            var newElements = elements
                .ReplaceIfExists(6, itm => itm == 4)
                .ToDictionary(itm => itm);

            Assert.AreEqual(5, newElements.Count);
            Assert.IsTrue(newElements.ContainsKey(1));
            Assert.IsTrue(newElements.ContainsKey(2));
            Assert.IsTrue(newElements.ContainsKey(3));
            Assert.IsFalse(newElements.ContainsKey(4));
            Assert.IsTrue(newElements.ContainsKey(5));
            Assert.IsTrue(newElements.ContainsKey(6));
        }
    }
}