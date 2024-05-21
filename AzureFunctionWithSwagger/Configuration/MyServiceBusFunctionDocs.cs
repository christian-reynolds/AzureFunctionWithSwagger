using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AzureFunctionWithSwagger.Configuration
{
    public static class MyServiceBusFunctionDocs
    {
        [OpenApiOperation(operationId: "ServiceBusFunction", tags: new[] { "servicebus" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "The Service Bus message payload")]
        public static void Run(
            [ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")] string myQueueItem,
            ILogger log)
        {
            // This method is just for OpenAPI documentation and should not contain actual implementation
        }
    }
}
