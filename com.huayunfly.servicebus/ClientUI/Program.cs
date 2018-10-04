using System;
using System.Threading.Tasks;
using com.huayunfly.servicebus.messages;
using NServiceBus;
using NServiceBus.Logging;

namespace ClientUI
{

    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();

        private static readonly string _storagePath = "D:\\";

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "ClientUI";

            // "ClientUI" is endpoint name, which serves as logic identity for endpoint.
            // It also forms a naming convention by other service, like "input queue".
            var endpointConfiguration = new EndpointConfiguration("ClientUI");

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
           
            // This is logical routing, the mapping of specific message types to logical endpoints 
            // that can process those messages. Each command message should have one logical endpoint 
            // that owns that message and can process it.

            // Within one logical endpoint, there may be many physical endpoint instances deployed 
            // to multiple servers.
            var routing = transport.Routing();

            #region Manual routing
            routing.RouteToEndpoint(typeof(PlaceOrder).Assembly, "com.huayunfly.servicebus.messages", "Sales");
            routing.RouteToEndpoint(typeof(RequestDataMessage).Assembly, "com.huayunfly.servicebus.messages", "Sales");
            #endregion

            #region Physical routing with MSMQ
            var instanceMappingFile = routing.InstanceMappingFile();
            var fileSettings = instanceMappingFile.FilePath("instance-mapping.xml");
            fileSettings.RefreshInterval(TimeSpan.FromSeconds(45));
            #endregion

            // An 'endpoint' is a logical concept, defined by an endpoint name and associated code, 
            // that defines an owner responsible for processing messages.

            // An 'endpoint instance' is a physical instance of the endpoint deployed to a single server. 
            // Many endpoint instances may be deployed to many servers in order to scale out the 
            // processing of a high-volume message to multiple servers.
            var endpointInstance = await Endpoint.Start(endpointConfiguration).
                ConfigureAwait(false);

            await RunLoop(endpointInstance).ConfigureAwait(false);
            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place an order, Press 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        #region PlaceOrder
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command);
                        #endregion

                        #region RequestDataMessage
                        var requestDataMessage = new RequestDataMessage()
                        {
                            DataId = Guid.NewGuid(),
                            String = "Place a new order"
                        };
                        log.Info($"Sending request data message, DataId = {requestDataMessage.DataId}");
                        await endpointInstance.Send(requestDataMessage);
                        #endregion
                        break;
                    case ConsoleKey.Q:
                        return;
                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }

        }
    }
}
