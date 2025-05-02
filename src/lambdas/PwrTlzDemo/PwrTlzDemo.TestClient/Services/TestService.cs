using Amazon.Lambda.Core;
using Amazon.Runtime.CredentialManagement;
using AutoFixture;
using AutoFixture.AutoMoq;
using dotenv.net.Utilities;
using Moq;
using Spectre.Console;

namespace PwrTlzDemo.TestClient.Services;

public static class TestService
{
    public static async Task RunAsync()
    {
        await InvokeLambdaFunctionAsync();
    }

    private static async Task InvokeLambdaFunctionAsync()
    {
        var autoFixture = new Fixture();
        autoFixture.Customize(
            new CompositeCustomization(
                new AutoMoqCustomization(),
                new SupportMutableValueTypesCustomization())
        );
        var mockLambdaContext = autoFixture.Freeze<Mock<ILambdaContext>>();

        var function = new Function();

        var results = await function.FunctionHandler(mockLambdaContext.Object);

        foreach(var item in results.Products)
            AnsiConsole.WriteLine(item.ToString());

        foreach(var item in results.Books)
            AnsiConsole.WriteLine(item.ToString());
    }

    public static async Task VerifySessionAsync()
    {
        var profileName = EnvReader.GetStringValue("AWS_PROFILE");

        if (!new CredentialProfileStoreChain().TryGetAWSCredentials(profileName, out _))
        {
            AnsiConsole.MarkupLine($"[red]You will need an active AWS session for the profile `{profileName}` to be able to run this test.[/]");
            Environment.Exit(1);
        }

        AnsiConsole.MarkupLine($"[green]`{profileName}` found. Verifying session...[/]");

        using var s3Client = new Amazon.S3.AmazonS3Client();
        try
        {
            await s3Client.ListBucketsAsync();
        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine("[red]Session verification failed. Confirm that you are signed in.[/]");
            Environment.Exit(1);
        }

        AnsiConsole.MarkupLine("[green]Session verified.[/]");
    }
}