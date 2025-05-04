namespace PwrTlzDemo.Models;

internal record DbCredentials
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}