using System;
using System.Collections.Generic;
using System.Text;

namespace com.huayunfly.app
{
    class BubbleSorter
    {
        static IList<T> BubbleSort<T>(IList<T> sortArray, Func<T, T, bool> comparisor)
        {
            bool sorted = false;
            for (int i = sortArray.Count - 1; i > 0; i--)
            {
                sorted = false;
                for (int j = 0; j < i; j++)
                {
                    if (comparisor(sortArray[j], sortArray[j+1]))
                    {
                        T temp = sortArray[i];
                        sortArray[i] = sortArray[j + 1];
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
