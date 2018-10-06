using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration.Logging;


namespace com.huayunfly.servicetransit
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            ConfigureLogger();
            Log4NetLogger.Use();

            IBusControl busControl = CreateBus();
            await busControl.StartAsync().ConfigureAwait(false);
            try
            {
                var requestClient = CreateRequestClient(busControl);
                await RunLoop(requestClient).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                await busControl.StopAsync().ConfigureAwait(false);
            }
        }

        static async Task RunLoop(IRequestClient<ISimpleRequest, ISimpleResponse> requestClient)
        {
            while (true)
            {
                Console.WriteLine("Press 'P' to place an order, Press 'Q' to quit.");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        var request = new SimpleRequest(Guid.NewGuid().ToString());
                        Console.WriteLine($"Sending SimpleRequest Message with CustomId {request.CustomId} ...");
                        ISimpleResponse response = 
                            await requestClient.Request(request).ConfigureAwait(false);
                        Console.WriteLine($"Received SimpleReponse Message with CustomName <{response.CustomName}>");
                        break;
                    case ConsoleKey.Q:
                        return;
                    default:
                        Console.WriteLine("Unknown input. Please try again.");
                        break;
                }

            }
        }

        static void ConfigureLogger()
        {
            const string logConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<log4net>
  <root>
    <level value=""INFO"" />
    <appender-ref ref=""console"" />
  </root>
  <appender name=""console"" type=""log4net.Appender.ColoredConsoleAppender"">
    <layout type=""log4net.Layout.PatternLayout"">
      <conversionPattern value=""%m%n"" />
    </layout>
  </appender>
</log4net>";
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(logConfig)))
            {
                XmlConfigurator.Configure(stream);
            }
        }

        static IBusControl CreateBus()
        {
            var uri = new Uri(ConfigurationManager.AppSettings["RabbitMQHost"] ?? "Uri not found");
            return Bus.Factory.CreateUsingRabbitMq(cfg => cfg.Host(uri, configurator =>
            {
                configurator.Username("guest");
                configurator.Password("guest");
            }));
        }

        static IRequestClient<ISimpleRequest, ISimpleResponse> CreateRequestClient(IBusControl busControl)
        {
            var serviceAddress = new Uri(ConfigurationManager.AppSettings["SerivceAddress"] ?? "Address not found");
            return busControl.CreateRequestClient<ISimpleRequest, ISimpleResponse>(
                serviceAddress, TimeSpan.FromSeconds(10));
        }
    }

    public class SimpleRequest : ISimpleRequest
    {
        public SimpleRequest(string customID)
        {
            CustomId = customID;
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; }

        public string CustomId { get; }
    }

    public class SimpleResponse : ISimpleResponse
    {
        public string CustomName { get; set; }
    }
}
