using NServiceBus;
using com.huayunfly.servicebus.messages;
using System.Threading.Tasks;
using NServiceBus.Logging;

namespace com.huayunfly.servicebus
{
    public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>
    {
        static ILog log = LogManager.GetLogger<RequestDataMessageHandler>();

        public async Task Handle(RequestDataMessage message, IMessageHandlerContext context)
        {
            log.Info($"Received request: {message.DataId}");
            log.Info($"String received: {message.String}");

            var response = new ResponseDataMessage()
            {
                DataId = message.DataId,
                String = "Placed a new order.",
            };

            log.Info($"Sending response data message, DataId = {response.DataId}");
            await context.Reply(response).ConfigureAwait(false);          
        }
    }
}
