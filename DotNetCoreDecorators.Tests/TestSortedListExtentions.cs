using System.Collections.Generic;
using NUnit.Framework;

namespace DotNetCoreDecorators.Tests
{
    public class TestSortedListExtentions
    {


        [Test]
        public void TestEqualCase()
        {
            var testList = new SortedList<int, int>
            {
                {1, 1},
                {12, 12},
                {5, 5},
                {6, 6},
                {20, 20}
            };

            var index = testList.SearchEqualOrGreater(5, (v1, v2) =>
            {
                if (v1 < v2)
                    return -1;
                return v1 > v2 ? 1 : 0;
            });

            Assert.AreEqual(1, index);
        }
        
        
        
        [Test]
        public void TestBeforeCase()
        {
            var testList = new SortedList<int, int>
            {
                {1, 1},
                {12, 12},
                {5, 5},
                {6, 6},
                {20, 20}
            };

            var index = testList.SearchEqualOrGreater(7, (v1, v2) =>
            {
                if (v1 < v2)
                    return -1;
                return v1 > v2 ? 1 : 0;
            });


            Assert.AreEqual(3, index);
        }
        
        [Test]
        public void TestBeyondTheUpBorderCase()
        {
            var testList = new SortedList<int, int>
            {
                {1, 1},
                {12, 12},
                {5, 5},
                {6, 6},
                {20, 20}
            };

            var index = testList.SearchEqualOrGreater(21);


            Assert.AreEqual(5, index);
        }
    }
}