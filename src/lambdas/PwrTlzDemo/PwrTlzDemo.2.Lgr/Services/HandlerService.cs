namespace PwrTlzDemo.Services;

internal class HandlerService
{
    //private readonly RegistrationService _registrationService;

    //private readonly LibraryService _libraryService;

    public HandlerService(
        //RegistrationService registrationService,
        //LibraryService libraryService
        )
    {
        //_registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
        //_libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
    }

    //[Logging(Service = "HandlerService")]
    public async Task<RegistrationResponse> ExecuteAsync(RegistrationRequest request)
    {
        //Logger.LogInformation("Getting library search response");

        // var librarySearchResponse = await _libraryService
        //     .GetBooksAsync();
        //
        // var randomBook = librarySearchResponse.Docs!
        //     .OrderBy(_ => Guid.NewGuid())
        //     .First();
        //
        // Logger.LogInformation("Adding registration");
        //
        // var registration = await _registrationService.RegisterAsync(request, randomBook);
        //
        // Logger.LogInformation("Registration completed");

        return new RegistrationResponse
        {
            Success = true,
            Message = "Registration successful",
            Registration = new Registration
            {
                Email = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                AddressLine1 = string.Empty,
                AddressLine2 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Zip = string.Empty,
                RegistrationDate = DateTimeOffset.Now,
                BookTitle = string.Empty
            }
            //Registration = registration
        };
    }
}