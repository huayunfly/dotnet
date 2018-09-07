using System;
using System.Collections.Generic;
using System.Text;

namespace com.huayunfly.app
{
    public class BubbleSorter
    {
        /**
         * Bubble sort method. For example: [1, 7, 2] -> [1, 2, 7]
         */
        public static IList<T> BubbleSort<T>(IList<T> sortArray, Func<T, T, bool> comparisor)
        {
            if (sortArray == null)
            {
                throw new ArgumentNullException("sortArray");
            }
            if (sortArray.Count < 1)
            {
                throw new ArgumentOutOfRangeException("sortArray", "Array count < 1");
            }
            bool sorted = false;
            for (int i = sortArray.Count - 1; i > 0; i--)
            {
                sorted = false;
                for (int j = 0; j < i; j++)
                {
                    if (comparisor(sortArray[j], sortArray[j+1]))
                    {
                        T temp = sortArray[j];
                        sortArray[j] = sortArray[j + 1];
                        sortArray[j + 1] = temp;
                        sorted = true;
                    }
                }
                if (!sorted) break;
            }
            return sortArray;
        }
    }
}
