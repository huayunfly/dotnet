using System;
using System.Threading.Tasks;
using com.huayunfly.servicebus.messages;
using NServiceBus;


namespace com.huayunfly.servicebus
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
            Console.Title = "Billing";

            // "Sales" is endpoint name, which serves as logic identity for endpoint.
            // It also forms a naming convention by other service, like "input queue".
            // Endpoint name is Case Sensitive.
            var endpointConfiguration = new EndpointConfiguration("Billing");
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

            // Within one logical endpoint, there may be many physical endpoint instances deployed 
            // to multiple servers.
            var routing = transport.Routing();

            #region Manual subscribe message routing
            routing.RegisterPublisher(typeof(OrderPlaced).Assembly, "com.huayunfly.servicebus.messages", "Sales");
            #endregion

            #region Physical routing with MSMQ
            var instanceMappingFile = routing.InstanceMappingFile();
            var fileSettings = instanceMappingFile.FilePath("instance-mapping.xml");
            fileSettings.RefreshInterval(TimeSpan.FromSeconds(45));
            #endregion

            var endpointInstance = await Endpoint.Start(endpointConfiguration).
                ConfigureAwait(false);
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
