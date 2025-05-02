using Amazon.Lambda.Core;
using AWS.Lambda.Powertools.Logging;
using AWS.Lambda.Powertools.Metrics;
using AWS.Lambda.Powertools.Tracing;
using Microsoft.Extensions.DependencyInjection;
using PwrTlzDemo.Models;
using PwrTlzDemo.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

/// <summary>
/// Learn more about Powertools for AWS Lambda (.NET) at https://awslabs.github.io/aws-lambda-powertools-dotnet/
/// Powertools for AWS Lambda (.NET) Logging: https://awslabs.github.io/aws-lambda-powertools-dotnet/core/logging/
/// Powertools for AWS Lambda (.NET) Tracing: https://awslabs.github.io/aws-lambda-powertools-dotnet/core/tracing/
/// Powertools for AWS Lambda (.NET) Metrics: https://awslabs.github.io/aws-lambda-powertools-dotnet/core/metrics/
/// Metrics Namespace and Service can be defined through Environment Variables https://awslabs.github.io/aws-lambda-powertools-dotnet/core/metrics/#getting-started
/// </summary>
public class Function
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    [Metrics(CaptureColdStart = true, Namespace = "PwrTlzDemo")]
    [Tracing]
    public async Task<IEnumerable<Product>> FunctionHandler(ILambdaContext context)
    {
        var entryPointService = ServiceProviderBuilder
            .Build()
            .GetRequiredService<ProductsService>();

        var result = await entryPointService.ExecuteAsync();

        return result.Take(5);
    }
}