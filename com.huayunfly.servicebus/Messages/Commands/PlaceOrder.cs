

using NServiceBus;

namespace com.huayunfly.servicebus.messages
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
    }

    public class OrderPlaced : IEvent
    {
        public string OrderId { get; set; }
    }
}
