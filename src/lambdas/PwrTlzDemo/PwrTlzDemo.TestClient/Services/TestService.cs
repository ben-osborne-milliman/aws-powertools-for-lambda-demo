using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS.Model;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using dotenv.net.Utilities;
using Moq;
using PwrTlzDemo.Library.Messaging;
using Spectre.Console;
using Spectre.Console.Json;

namespace PwrTlzDemo.TestClient.Services;

public static class TestService
{
    public static async Task RunAsync()
    {
        var options = new Dictionary<string, Func<Task>>()
        {
            { "Invoke Lambda Function", InvokeLambdaFunctionAsync },
            { "Add Messages to Queue", AddMessagesToQueueAsync }
        };

        var selectedKey = await AnsiConsole.PromptAsync(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices(options.Keys)
            );

        AnsiConsole.MarkupLine($"[green]You selected: {selectedKey}[/]");

        var selectedOption = options[selectedKey];
        await selectedOption();
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

        var request = GenerateRandomRegistrationRequest();
        var requestJson  = new JsonText(Serialize(request));

        AnsiConsole.Write(
            new Panel(requestJson)
                .Header("Request")
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Blue));

        var result = await function.FunctionHandler(request, mockLambdaContext.Object);
        var resultJson  = new JsonText(JsonSerializer.Serialize(result, JsonOptions));

        AnsiConsole.Write(
            new Panel(resultJson)
                .Header("Response")
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Green));
    }

    private static async Task AddMessagesToQueueAsync()
    {
        var sqsClient = new Amazon.SQS.AmazonSQSClient();
        var queueUrl = EnvReader.GetStringValue("SQS_QUEUE_URL");

        var messageCount = await AnsiConsole.PromptAsync(
            new TextPrompt<int>("How many messages would you like to add?")
                .PromptStyle("green")
                .DefaultValue(10)
                .Validate(input => input is > 0 and < 100)
        );

        var msgGroupId = $"pwrtzldemo-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

        for (var i = 0; i < messageCount; i++)
        {
            var msg = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = Serialize(GenerateRandomRegistrationRequest()),
                MessageGroupId = msgGroupId,
                MessageDeduplicationId = $"{msgGroupId}-{i}"
            };
            var response = await sqsClient.SendMessageAsync(msg);
            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                AnsiConsole.WriteLine($"Message {i + 1} sent successfully. Message ID: {response.MessageId}");
            else
            {
                AnsiConsole.MarkupLine($"[red]Failed to send message {i + 1}. Status code: {response.HttpStatusCode}[/]");
                break;
            }
        }
    }

    private static RegistrationRequest GenerateRandomRegistrationRequest()
    {
        var request = new Faker<RegistrationRequest>()
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.FirstName, f => f.Name.FirstName())
            .RuleFor(x => x.LastName, f => f.Name.LastName())
            .RuleFor(x => x.AddressLine1, f => f.Address.StreetAddress())
            .RuleFor(x => x.AddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(x => x.City, f => f.Address.City())
            .RuleFor(x => x.State, f => f.Address.State())
            .RuleFor(x => x.Zip, f => f. Address.ZipCode())
            .RuleFor(x => x.RegistrationDate, f => f.Date.Past())
            .Generate();
        return request;
    }

    private static string Serialize<T>(T request)
    {
        var requestJson  = JsonSerializer.Serialize(request, JsonOptions);
        return requestJson;
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