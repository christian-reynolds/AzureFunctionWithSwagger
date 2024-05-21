using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace AzureFunctionWithSwagger
{
    public class ServiceBusFunction
    {
//        /// <summary>
//        /// ServiceBusTrigger function
//        /// </summary>
//        /// <param name="myQueueItem"></param>
//        /// <param name="log"></param>
//        [FunctionName("ServiceBusFunction")]
//        public void Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")] string myQueueItem, ILogger log)
//        {
//#if DEBUG
//            // Simulate Service Bus message processing locally
//            log.LogInformation("C# ServiceBus queue trigger function processed a simulated message.");
//#else
//        log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
//#endif
//        }

        /// <summary>
        /// ServiceBusTrigger function documentation.  This is only used for OpenApi documentation purposes!
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("DocumentServiceBusFunction")]
        [OpenApiOperation(operationId: "ServiceBusFunction", tags: new[] { "servicebus" })]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ServiceBusMessage), Required = true, Description = "The Service Bus message payload")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static IActionResult DocumentRun(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "servicebus/doc")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Documenting Service Bus function for OpenAPI.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<ServiceBusMessage>(requestBody);

            // No actual processing; this method is for OpenAPI documentation only
            return new OkObjectResult($"Received message: {data.Message}");
        }

        public class ServiceBusMessage
        {
            public string Message { get; set; }
        }
    }
}
