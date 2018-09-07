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
    }
}
