using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunctionWithSwagger
{
    public class ServiceBusFunction
    {
        [FunctionName("ServiceBusFunction")]
        public void Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")] string myQueueItem, ILogger log)
        {
#if DEBUG
            // Simulate Service Bus message processing locally
            log.LogInformation("C# ServiceBus queue trigger function processed a simulated message.");
#else
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
#endif
        }
    }
}
