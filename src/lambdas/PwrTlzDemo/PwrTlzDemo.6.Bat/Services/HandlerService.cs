namespace PwrTlzDemo.Services;

internal class HandlerService
{
    private readonly RegistrationService _registrationService = new();

    private readonly LibraryService _libraryService = new();

    [Tracing]
    [Logging(Service = "HandlerService")]
    public async Task<RegistrationResponse> ExecuteAsync(RegistrationRequest request)
    {
        Logger.LogInformation("Getting library search response");

        var librarySearchResponse = await _libraryService
            .GetBooksAsync();

        var randomBook = librarySearchResponse.Docs!
            .OrderBy(_ => Guid.NewGuid())
            .First();

        Logger.LogInformation("Adding registration");

        var registration = await _registrationService.RegisterAsync(request, randomBook);

        Logger.LogInformation("Registration completed");

        return new RegistrationResponse
        {
            Success = true,
            Message = "Registration successful",
            Registration = registration
        };
    }
}