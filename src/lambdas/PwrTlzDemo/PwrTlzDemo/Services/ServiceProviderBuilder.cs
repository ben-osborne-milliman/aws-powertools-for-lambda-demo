using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.XRay.Recorder.Handlers.System.Net;
using IntelliScript.RdsPgConnector.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PwrTlzDemo.Providers;

namespace PwrTlzDemo.Services;

internal static class ServiceProviderBuilder
{
    public static IServiceProvider Build() =>
        new ServiceCollection()
            .AddLogging(builder => builder.AddLambdaLogger())
            .AddScoped<HttpClientXRayTracingHandler>()
            .AddHttpClient().ConfigureHttpClientDefaults(configure =>
            {
                configure.AddHttpMessageHandler<HttpClientXRayTracingHandler>();
            })
            .AddSingleton<EcommerceDataProvider>()
            .AddSingleton<ProductsService>()
            .ConfigureRdsPgConnector(
                () => GetAwsCredentials(),
                () => RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION")))
            .BuildServiceProvider();

    private static readonly Func<AWSCredentials> GetAwsCredentials = () =>
    {
        var profileName = Environment.GetEnvironmentVariable("AWS_PROFILE");

        if (string.IsNullOrEmpty(profileName))
            return FallbackCredentialsFactory.GetCredentials();

        var profileCredentials =
            new CredentialProfileStoreChain().TryGetAWSCredentials(profileName, out var profileCredential);

        if (profileCredentials)
            return profileCredential;

        throw new InvalidOperationException("Unable to get AWS credentials");
    };
}

