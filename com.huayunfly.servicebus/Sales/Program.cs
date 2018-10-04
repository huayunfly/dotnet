using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;


namespace Sales
{
    class Program
    {
        private static readonly string _storagePath = "D:\\";

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            // Define INeedInitialization interface for repetitive code.
            Console.Title = "Sales";

            // "Sales" is endpoint name, which serves as logic identity for endpoint.
            // It also forms a naming convention by other service, like "input queue".
            var endpointConfiguration = new EndpointConfiguration("sales");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            // Set transport storage directory using resourse.resx
            transport.StorageDirectory(_storagePath);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).
                ConfigureAwait(false);
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            await endpointInstance.Stop().ConfigureAwait(false);       
        }
    }
}
