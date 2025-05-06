[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

public class Function
{
    private readonly ServiceProvider _serviceProvider;

    public Function()
    {
        _serviceProvider = BuildServiceProvider();
    }

    public async Task<RegistrationResponse> FunctionHandler(RegistrationRequest request, ILambdaContext context)
    {
        Logger.LogInformation("FunctionHandler invoked");

        var handler = _serviceProvider
            .GetRequiredService<HandlerService>();
        return await handler.ExecuteAsync(request);
    }

    private ServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddHttpClient()
            //.AddSingleton<EcommerceDataProvider>()
            //.AddSingleton<RegistrationService>()
            //.AddSingleton<LibraryService>()
            .AddSingleton<HandlerService>()
            .BuildServiceProvider();
}