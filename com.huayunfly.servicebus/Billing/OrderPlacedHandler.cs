using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.huayunfly.servicebus.messages;
using NServiceBus.Logging;

namespace com.huayunfly.servicebus
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static private ILog log = LogManager.GetLogger<OrderPlacedHandler>();
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");
            return Task.CompletedTask;
        }
    }
}
