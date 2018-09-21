using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using System.Net.Http;

namespace com.huayunfly.app
{
    /* Using logman to record events:
     * step1: cmd >logman start mysession -p {049a928c-040f-59fb-561d-5d329f0a774e} -o mytrace.etl -ets
     * setp2: Start app with EventSource.
     * step3: cmd >logman stop mysession -ets
     * 
     */
    public class SimpleEventSourceSample
    {
        private static EventSource sampleEventSource = new EventSource("hy-eventSourceSample");
        public static async Task NetworkRequestSample()
        {
            /* You need to first start the application to copy the GUID which 
             * created for the application. The GUID is for logman. */
            Console.WriteLine($"Log GUID: {sampleEventSource.Guid}");
            try
            {
                Console.WriteLine("Begin url requesting....");
                using (var client = new HttpClient())
                {
                    string url = "http://www.bing.com";
                    sampleEventSource.Write("Network", new { Info = $"requesting {url}" }); /* Anonymous type */
                    string result = await client.GetStringAsync(url);
                    sampleEventSource.Write("Network", 
                        new { Info = $"completed call to {url}, result string length {result.Length}" });
                }
                Console.WriteLine("Complete................");

            }
            catch (Exception ex)
            {
                /* Set event level here. */
                sampleEventSource.Write("Network Error", 
                    new EventSourceOptions { Level = EventLevel.Error },
                    new { Message = ex.Message, Result=ex.HResult});
                Console.WriteLine(ex.Message);
            }
        }

        public static void Dispose()
        {
            sampleEventSource?.Dispose();
        }
    }

    public class SampleEventSource : EventSource
    {
        private SampleEventSource() : base("hy-SampleEventSource") { }

        public static SampleEventSource Log = new SampleEventSource();

        public void Startup() => WriteEvent(1);

        public void CallService(string url) => WriteEvent(2, url);

        public void CalledService(string url, int length) => WriteEvent(3, url, length);

        public void ServiceError(string message, int error) => WriteEvent(4, message, error);

    }
}
