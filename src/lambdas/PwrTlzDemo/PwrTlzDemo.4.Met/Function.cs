[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

public class Function
{
    private readonly ServiceProvider _serviceProvider;

    public Function()
    {
        Tracing.RegisterForAllServices();

        _serviceProvider = BuildServiceProvider();
    }

    [Metrics(CaptureColdStart = true, Namespace = nameof(PwrTlzDemo))]
    [Tracing]
    public async Task<RegistrationResponse> FunctionHandler(RegistrationRequest request, ILambdaContext context)
    {
        Logger.LogInformation("FunctionHandler invoked");

        var handler = _serviceProvider
            .GetRequiredService<HandlerService>();
        return await handler.ExecuteAsync(request);
    }

    [Tracing]
    private ServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddScoped<HttpClientXRayTracingHandler>()
            .AddHttpClient().ConfigureHttpClientDefaults(configure =>
            {
                configure.AddHttpMessageHandler<HttpClientXRayTracingHandler>();
            })
            .AddSingleton<EcommerceDataProvider>()
            .AddSingleton<RegistrationService>()
            .AddSingleton<LibraryService>()
            .AddSingleton<HandlerService>()
            .BuildServiceProvider();
}