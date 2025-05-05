using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Runtime.CredentialManagement;
using AutoFixture;
using AutoFixture.AutoMoq;
using dotenv.net.Utilities;
using Moq;
using PwrTlzDemo.Messaging;
using Spectre.Console;
using Spectre.Console.Json;

namespace PwrTlzDemo.TestClient.Services;

public static class TestService
{
    public static async Task RunAsync() =>
        await InvokeLambdaFunctionAsync();

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

        var request = autoFixture.Create<RegistrationRequest>();
        var result = await function.FunctionHandler(request, mockLambdaContext.Object);
        var json  = new JsonText(JsonSerializer.Serialize(result, JsonOptions));

        AnsiConsole.Write(
            new Panel(json)
                .Header("Result")
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Yellow));
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

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
}