using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.huayunfly.app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hello World!");
            LINQQuery();
            BubbleSort();
            EventSourceSampleAsync();
            Console.ReadLine();
        }

        private static void LINQQuery()
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

        private static void BubbleSort()
        {
            IList<int> sortArray = new List<int> { 9, 1, 7, 3, 5, 4 };
            IList<int> sorted = BubbleSorter.BubbleSort(sortArray, (a, b) => a < b);
            string formatArrayString = 
                string.Join(",", sortArray.Select(x => x.ToString()).ToArray());
            Console.WriteLine(formatArrayString);
        }

        private static async Task EventSourceSampleAsync()
        {
            await SimpleEventSourceSample.NetworkRequestSample();
            SimpleEventSourceSample.Dispose();
        }

        private static void LogSampleUsingDI()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddEventSourceLogger();
                builder.AddConsole();
#if DEBUG
                builder.AddDebug();
#endif
            });

        }

    }
}
