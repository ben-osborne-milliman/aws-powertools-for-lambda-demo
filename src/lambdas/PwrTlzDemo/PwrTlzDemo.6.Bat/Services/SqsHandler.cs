using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.BatchProcessing;
using AWS.Lambda.Powertools.BatchProcessing.Sqs;

namespace PwrTlzDemo.Services;

internal class SqsHandler : ISqsRecordHandler
{
    private readonly RegistrationService _registrationService = new();

    private readonly LibraryService _libraryService = new();

    public Task<RecordHandlerResult> HandleAsync(SQSEvent.SQSMessage record, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}