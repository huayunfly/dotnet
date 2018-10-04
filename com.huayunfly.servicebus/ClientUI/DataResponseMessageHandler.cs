using System.Threading.Tasks;
using com.huayunfly.servicebus.messages;
using NServiceBus;
using NServiceBus.Logging;

namespace com.huayunfly.servicebus
{
    public class DataResponseMessageHandler : IHandleMessages<ResponseDataMessage>
    {
        static ILog log = LogManager.GetLogger<DataResponseMessageHandler>();
        public Task Handle(ResponseDataMessage message, IMessageHandlerContext context)
        {
            log.Info($"Response received: {message.DataId}");
            log.Info($"Received string: {message.String}");
            return Task.CompletedTask;
        }
    }
}
