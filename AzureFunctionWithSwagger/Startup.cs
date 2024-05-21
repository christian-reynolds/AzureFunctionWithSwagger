using AzureFunctionWithSwagger.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFunctionWithSwagger.Startup))]

namespace AzureFunctionWithSwagger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register your IOpenApiConfigurationOptions implementation
            builder.Services.AddSingleton<IOpenApiConfigurationOptions, OpenApiConfigurationOptions>();
        }
    }
}
