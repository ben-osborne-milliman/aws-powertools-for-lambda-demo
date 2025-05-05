using PwrTlzDemo.Library.Models;

namespace PwrTlzDemo.Library.Messaging;

public record RegistrationResponse
{
    public required bool Success { get; init; }

    public required string Message { get; init; }

    public Registration? Registration { get; init; }
}