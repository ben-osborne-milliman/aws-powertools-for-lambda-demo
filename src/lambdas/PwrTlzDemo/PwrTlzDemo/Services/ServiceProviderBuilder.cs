using Amazon.XRay.Recorder.Handlers.System.Net;
using AWS.Lambda.Powertools.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PwrTlzDemo.Providers;

namespace PwrTlzDemo.Services;

internal static class ServiceProviderBuilder
{
    [Tracing]
    public static IServiceProvider Build()
    {
        var serviceCollection = new ServiceCollection()
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
        return serviceCollection;
    }
}

