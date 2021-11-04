using NUnit.Framework;

namespace DotNetCoreDecorators.Tests
{
    public class ArrayUtilsTest
    {

        
        [Test]
        public static void TestSplitByChunks()
        {
            var array = new[] {0, 1, 2, 3, 4, 5, 6};

            var chunks = array.SplitToChunks(2).AsReadOnlyList();
            
            
            Assert.AreEqual(4, chunks.Count);

            Assert.AreEqual(2, chunks[0].Count);
            Assert.AreEqual(2, chunks[1].Count);
            Assert.AreEqual(2, chunks[2].Count);
            Assert.AreEqual(1, chunks[3].Count);
            
            
            Assert.AreEqual(0, chunks[0][0]);
            Assert.AreEqual(1, chunks[0][1]);
            Assert.AreEqual(2, chunks[1][0]);
            Assert.AreEqual(3, chunks[1][1]);
            Assert.AreEqual(4, chunks[2][0]);
            Assert.AreEqual(5, chunks[2][1]);
            Assert.AreEqual(6, chunks[3][0]);

        }
    }
}