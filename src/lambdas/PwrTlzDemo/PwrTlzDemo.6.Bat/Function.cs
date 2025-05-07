using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.BatchProcessing;
using AWS.Lambda.Powertools.BatchProcessing.Sqs;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace PwrTlzDemo;

public class Function
{
    public Function()
    {
        Tracing.RegisterForAllServices();
    }

    [Metrics(CaptureColdStart = true, Namespace = nameof(PwrTlzDemo))]
    [Tracing]
    [BatchProcessor(RecordHandler = typeof(SqsHandler),
        ErrorHandlingPolicy = BatchProcessorErrorHandlingPolicy.StopOnFirstBatchItemFailure)]
    public BatchItemFailuresResponse FunctionHandler(SQSEvent _, ILambdaContext lambdaContext)
    {
        return SqsBatchProcessor.Result.BatchItemFailuresResponse;
    }
}