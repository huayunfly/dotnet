using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.huayunfly.servicebus.messages;
using NServiceBus;
using NServiceBus.Logging;

namespace com.huayunfly.servicebus
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();

        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info("Received PlaceOrder, OrderId=" + message.OrderId);
            return Task.CompletedTask;
        }
    }
}
