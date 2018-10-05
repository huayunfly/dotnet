using System;
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
            // Endpoint name is Case Sensitive.
            var endpointConfiguration = new EndpointConfiguration("Sales");
            #region ConfigureLearningTransport
            // Set transport storage directory for learningtransport
            // var transport = endpointConfiguration.UseTransport<LearningTransport>();
            // transport.StorageDirectory(_storagePath);
            #endregion

            #region ConfigureMsmqTransport
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            // transport.DisableInstaller();
            #endregion
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).
                ConfigureAwait(false);
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            await endpointInstance.Stop().ConfigureAwait(false);       
        }
    }
}
