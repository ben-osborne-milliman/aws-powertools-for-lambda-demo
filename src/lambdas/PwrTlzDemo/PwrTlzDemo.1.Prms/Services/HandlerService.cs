namespace PwrTlzDemo.Services;

internal class HandlerService
{
    private readonly RegistrationService _registrationService;

    private readonly LibraryService _libraryService;

    public HandlerService(RegistrationService registrationService, LibraryService libraryService)
    {
        _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
        _libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
    }

    public async Task<RegistrationResponse> ExecuteAsync(RegistrationRequest request)
    {
        var librarySearchResponse = await _libraryService
            .GetBooksAsync();

        var randomBook = librarySearchResponse.Docs!
            .OrderBy(_ => Guid.NewGuid())
            .First();

        var registration = await _registrationService.RegisterAsync(request, randomBook);

        return new RegistrationResponse
        {
            Success = true,
            Message = "Registration successful",
            Registration = registration
        };
    }
}