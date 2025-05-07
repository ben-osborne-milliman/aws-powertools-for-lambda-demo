using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.BatchProcessing;
using AWS.Lambda.Powertools.BatchProcessing.Sqs;

namespace PwrTlzDemo.Services;

internal class SqsHandler : ISqsRecordHandler
{
    private readonly HandlerService _handlerService = new();

    [Tracing]
    [Logging(Service = "SqsHandler")]
    public async Task<RecordHandlerResult> HandleAsync(SQSEvent.SQSMessage record, CancellationToken cancellationToken)
    {
        if (new Random().Next(1, 101) <= 15) // 15% chance
        {
            Logger.LogWarning("Simulating random error");
            throw new AggregateException("Simulated random error");
        }

        var request = JsonSerializer.Deserialize<RegistrationRequest>(record.Body) ??
            throw new AggregateException("Could not deserialize message to RegistrationRequest");

        try
        {
            await _handlerService.ExecuteAsync(request);
            Logger.LogInformation("Successfully processed message with ID: {MessageId}", record.MessageId);
            return RecordHandlerResult.None;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error processing SQSEvent");
            throw;
        }
    }
}