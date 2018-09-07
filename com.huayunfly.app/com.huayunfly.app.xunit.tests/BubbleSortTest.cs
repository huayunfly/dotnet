using System;
using System.Collections.Generic;
using Xunit;

namespace com.huayunfly.app.xunit.tests
{
    public class BubbleSortTest
    {
        [Fact]
        public void BubbleSortingTest()
        {
            IList<int> sortArray = new List<int> { 9, 1, 7, 3, 5, 4 };
            IList<int> sorted = BubbleSorter.BubbleSort(sortArray, (a, b) => a < b);
            Assert.Equal(9, sorted[0]);
            Assert.Equal(1, sorted[5]);

            IList<int> nullArray = null;
            Assert.Throws<ArgumentNullException>(() =>
                BubbleSorter.BubbleSort(nullArray, (a, b) => a < b)
                );
        }
    }
}
