using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;

namespace com.huayunfly.servicetransit
{
    public class RequestConsumer : IConsumer<ISimpleRequest>
    {
        // Masstransit log
        private readonly ILog _log = Logger.Get<RequestConsumer>();

        // Comsume() is like NServiceBus's Endpoint Handle().
        public async Task Consume(ConsumeContext<ISimpleRequest> context)
        {
            _log.Info($"Return CustomName for {context.Message.CustomId}");
            throw new Exception("Very bad things happened");

            await context.RespondAsync(new SimpleResponse()
            { CustomName = $"Customer number {context.Message.CustomId}" }
            );
        }
    }

    class SimpleResponse : ISimpleResponse
    {
        public string CustomName { get; set;}
    }
}
