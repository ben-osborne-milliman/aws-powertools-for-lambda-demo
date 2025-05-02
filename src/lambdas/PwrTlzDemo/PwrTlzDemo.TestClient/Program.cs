using Amazon.Lambda.Core;
using AutoFixture;
using AutoFixture.AutoMoq;
using dotenv.net;
using Moq;
using PwrTlzDemo.TestClient.Services;

namespace PwrTlzDemo.TestClient;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        DotEnv.Load(new DotEnvOptions(envFilePaths: [
            "./.env.local"
        ]));

        await TestService.VerifySessionAsync();

        await TestService.RunAsync();
    }
}