namespace PwrTlzDemo.Messaging;

public record RegistrationRequest
{
    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string AddressLine1 { get; init; }

    public required string AddressLine2 { get; init; }

    public required string City { get; init; }

    public required string State { get; init; }

    public required string Zip { get; init; }

    public required DateTimeOffset RegistrationDate { get; init; }
}