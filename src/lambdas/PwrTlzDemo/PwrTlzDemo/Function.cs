using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;
using AWS.Lambda.Powertools.Metrics;
using AWS.Lambda.Powertools.Tracing;
using Microsoft.Extensions.DependencyInjection;
using PwrTlzDemo.Messaging;
using PwrTlzDemo.Models;
using PwrTlzDemo.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

public class Function
{
    [Metrics(CaptureColdStart = true, Namespace = "PwrTlzDemo")]
    [Tracing]
    public async Task<InventoryResponse> FunctionHandler(ILambdaContext context)
    {
        Tracing.RegisterForAllServices();

        var handler = ServiceProviderBuilder
            .Build()
            .GetRequiredService<HandlerService>();

        return await handler.ExecuteAsync();
  }
}