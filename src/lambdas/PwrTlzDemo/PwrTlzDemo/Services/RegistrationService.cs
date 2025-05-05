
namespace PwrTlzDemo.Services;

internal class RegistrationService
{
    private readonly EcommerceDataProvider _ecommerceDataProvider;

    public RegistrationService(EcommerceDataProvider ecommerceDataProvider, ILogger<RegistrationService> logger)
    {
        _ecommerceDataProvider =
            ecommerceDataProvider ?? throw new ArgumentNullException(nameof(ecommerceDataProvider));
    }

    [Tracing]
    [Logging(Service = "RegistrationService")]
    public async Task<Registration> RegisterAsync(RegistrationRequest request, Doc book)
    {
        var registration = new Registration
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            State = request.State,
            Zip = request.Zip,
            RegistrationDate = DateTimeOffset.UtcNow,
            BookTitle = book.Title!
        };

        Logger.LogInformation($"Registering book: {book.Title}");

        await _ecommerceDataProvider.InsertRegistrationAsync(registration);
        return registration;
    }
}