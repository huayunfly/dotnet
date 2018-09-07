using System;
using System.Linq;
using System.Collections.Generic;

namespace com.huayunfly.app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello World!");
            LINQQuery();
            BubbleSort();
        }

        static void LINQQuery()
        {
            var query = from r in Formula1.GetChampions()
                        where r.Country == "UK"
                        orderby r.Wins descending
                        select r;
            foreach (Racer r in query)
            {
                Console.WriteLine($"{r:A}");

            }
        }

        static void BubbleSort()
        {
            IList<int> sortArray = new List<int> { 9, 1, 7, 3, 5, 4 };
            IList<int> sorted = BubbleSorter.BubbleSort(sortArray, (a, b) => a < b);
            string formatArrayString = 
                string.Join(",", sortArray.Select(x => x.ToString()).ToArray());
            Console.WriteLine(formatArrayString);
        }

    }
}
