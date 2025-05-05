namespace PwrTlzDemo.Library.Models;

public record DbCredentials
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}