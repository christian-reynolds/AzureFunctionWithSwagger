
# Azure Function with Swagger

This is a sample Azure Function project configured with Swagger using the `Microsoft.Azure.WebJobs.Extensions.OpenApi` library.

## Project Structure

```
AzureFunctionWithSwagger/
├── .gitignore
├── AzureFunctionWithSwagger.csproj
├── Function1.cs
├── host.json
├── local.settings.json
├── ServiceBusFunction.cs
├── Startup.cs
└── README.md
├── Configuration/
    ├── OpenApiConfigurationOptions.cs
```

### Files

- **.gitignore**: Specifies files and directories to be ignored by Git.
- **AzureFunctionWithSwagger.csproj**: The project file for the Azure Function.
- **Function1.cs**: Contains the implementation of the example HTTP-triggered function.
- **host.json**: Configuration file for the function host.
- **local.settings.json**: Local settings file for running the function locally.
- **ServiceBusFunction.cs**: Contains the Service Bus function and the OpenAPI documentation function.
- **Startup.cs**: Configures services and middleware for the function app.
- **OpenApiConfigurationOptions.cs**: Custom OpenAPI configuration options.
- **README.md**: This readme file.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Azure Functions Core Tools](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
- An Azure account (for deployment)

## Getting Started

### Setup

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/AzureFunctionWithSwagger.git
    cd AzureFunctionWithSwagger
    ```

2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

3. Run the function locally:

    ```sh
    func start
    ```

### Function Implementation

#### HTTP Trigger Example (`Function1.cs`)

```csharp
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;

public static class Function1
{
    [FunctionName("Function1")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "example" })]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The name parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        return new OkObjectResult($"Hello, {name}");
    }
}
```

#### Service Bus Trigger Function (ServiceBusFunction.cs)

```csharp
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;

namespace AzureFunctionWithSwagger
{
    public static class MyServiceBusFunction
    {
#if !DEBUG
        [FunctionName("ServiceBusFunction")]
        public static void Run(
            [ServiceBusTrigger("myqueue", Connection = "ServiceBusConnection")] string myQueueItem,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
#endif

        [OpenApiOperation(operationId: "ServiceBusFunction", tags: new[] { "servicebus" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ServiceBusMessage), Required = true, Description = "The Service Bus message payload")]
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
    }

    public class ServiceBusMessage
    {
        public string Message { get; set; }
    }
}
```

### Configuration

#### `host.json`

The `host.json` file contains the global configuration options for all functions:

```json
{
    "version": "2.0",
    "logging": {
        "applicationInsights": {
            "samplingSettings": {
                "isEnabled": true,
                "excludedTypes": "Request"
            },
            "enableLiveMetricsFilters": true
        }
    }
}
```

#### `local.settings.json`

The `local.settings.json` file contains the configuration settings for running the function locally:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"ServiceBusConnection": "UseDevelopmentServiceBus=true"
    }
}
```

### Swagger Configuration

The `Startup.cs` file configures the Swagger setup:

```csharp
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register OpenAPI configuration options
            builder.Services.AddSingleton<IOpenApiConfigurationOptions, MyOpenApiConfigurationOptions>();
        }
    }
}
```

## Accessing SwaggerUI

After running the function locally, you can access the SwaggerUI at `http://localhost:7071/api/swagger/ui`.

## Deployment

Deploy the function to Azure using the Azure CLI or Visual Studio. Refer to the [official documentation](https://learn.microsoft.com/en-us/cli/azure/functionapp/deployment?view=azure-cli-latest) for more details.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue to discuss any changes.

## License

This project is licensed under the MIT License.
