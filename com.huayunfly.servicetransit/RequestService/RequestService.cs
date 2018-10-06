using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Topshelf;
using Topshelf.Logging;

namespace com.huayunfly.servicetransit
{
    public class RequestService : ServiceControl
    {
        // Topshelf log, for Masstransit log is not avaiable now.
        private readonly LogWriter _log = HostLogger.Get<RequestService>();

        private IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {
            AsyncStart().GetAwaiter().GetResult();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            AsyncStop().GetAwaiter().GetResult();
            return true;
        }

        private async Task AsyncStart()
        {
            _log.Info("Creating bus...");

            _busControl = CreateBus();

            _log.Info("Starting bus...");

            await _busControl.StartAsync().ConfigureAwait(false);
        }

        private async Task AsyncStop()
        {
            _log.Info("Stopping bus...");
            await _busControl.StopAsync().ConfigureAwait(false);
        }

        private IBusControl CreateBus()
        {
            var uri = new Uri(ConfigurationManager.AppSettings["RabbitMQHost"] ?? "Uri not found");
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                IRabbitMqHost host = cfg.Host(uri, configurator =>
                {
                    configurator.Username("guest");
                    configurator.Password("guest");
                });
                
                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["ServiceQueueName"],
                    configure => { configure.Consumer<RequestConsumer>(); });
            });
        }
    }
}
