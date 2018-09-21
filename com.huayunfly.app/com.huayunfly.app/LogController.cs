using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace com.huayunfly.app
{
    public class SampleLogController
    {
        private ILogger<SampleLogController> _logger;

        public SampleLogController(ILogger<SampleLogController> logger)
        {
            _logger = logger;
            _logger.LogTrace("ILogger injected into {0}", nameof(SampleLogController));

        } 

        public async Task NetworkRequestSampleAsync(string url)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.Networking, 
                    "NetworkRequestSampleAsync started with url {0}", url);
                var client = new HttpClient();

                string result = await client.GetStringAsync(url);
                _logger.LogInformation(LoggingEvents.Networking,
                    "NetworkRequestSampleAsync completed, receive {0} characters", result.Length);

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.Networking, ex,
                    "Error in NetworkRequestSampleAsync, error message: {0}, HResult: {1}",
                    ex.Message, ex.HResult);
            }
        }
    }

    struct LoggingEvents
    {
        public const int Injection = 2000;
        public const int Networking = 2001;
    }


}
