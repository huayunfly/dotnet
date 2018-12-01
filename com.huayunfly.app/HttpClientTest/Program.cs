using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient();

        public static void Log(string prefix) => 
            Console.WriteLine($"{prefix}, task: {Task.CurrentId}, " + 
                $"thread: {Thread.CurrentThread.ManagedThreadId}, time: {DateTime.Now}");

        static void Main(string[] args)
        {
            int NUMBER_OF_REQUESTS = 100;
            ServicePointManager.DefaultConnectionLimit = 100;

            Parallel.For(0, NUMBER_OF_REQUESTS, async i =>
            {
                HttpResponseMessage httpResponse = null;
                Log($"S {i}");
                httpResponse = await _httpClient.GetAsync("http://www.bing.com");
                Log($"E {i}");
            }
            );
            Console.ReadLine();
        }
    }
}
