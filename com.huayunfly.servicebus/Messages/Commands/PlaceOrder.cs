

using NServiceBus;

namespace com.huayunfly.servicebus.messages
{
    public class PlaceOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}
