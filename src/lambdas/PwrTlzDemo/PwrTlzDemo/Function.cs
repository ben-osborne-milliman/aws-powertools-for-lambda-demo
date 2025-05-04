using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Handlers.System.Net;
using AWS.Lambda.Powertools.Metrics;
using AWS.Lambda.Powertools.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PwrTlzDemo.Messaging;
using PwrTlzDemo.Providers;
using PwrTlzDemo.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

public class Function
{
    private ServiceProvider? _serviceProvider = null;

    [Metrics(CaptureColdStart = true, Namespace = "PwrTlzDemo")]
    [Tracing]
    public async Task<InventoryResponse> FunctionHandler(ILambdaContext context)
    {
        Tracing.RegisterForAllServices();

        var handler = GetServiceProvider()
            .GetRequiredService<HandlerService>();

        return await handler.ExecuteAsync();
    }

    [Tracing]
    private ServiceProvider GetServiceProvider() =>
        _serviceProvider ??= BuildServiceProvider();

    [Tracing]
    private ServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddLogging(builder => builder.AddLambdaLogger())
            .AddScoped<HttpClientXRayTracingHandler>()
            .AddHttpClient().ConfigureHttpClientDefaults(configure =>
            {
                configure.AddHttpMessageHandler<HttpClientXRayTracingHandler>();
            })
            .AddSingleton<EcommerceDataProvider>()
            .AddSingleton<ProductsService>()
            .AddSingleton<LibraryService>()
            .AddSingleton<HandlerService>()
            .BuildServiceProvider();
}