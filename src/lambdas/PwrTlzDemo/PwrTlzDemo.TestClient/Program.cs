using dotenv.net;
using PwrTlzDemo.TestClient.Services;

namespace PwrTlzDemo.TestClient;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        DotEnv.Load(new DotEnvOptions(envFilePaths: [
            "./.env.dev"
            //"./.env.local"
        ]));

        await TestService.VerifySessionAsync();

        await TestService.RunAsync();
    }
}