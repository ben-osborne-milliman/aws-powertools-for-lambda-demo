using System.Text.Json;
using AWS.Lambda.Powertools.Parameters;
using AWS.Lambda.Powertools.Tracing;
using Dapper;
using dotenv.net.Utilities;
using Npgsql;
using PwrTlzDemo.Models;

namespace PwrTlzDemo.Providers;

internal class EcommerceDataProvider
{
    public EcommerceDataProvider()
    {
    }

    [Tracing]
    public async Task InsertRegistrationAsync(Registration registration)
    {
        await using var connection = await GetConnectionAsync();
        await InsertRegistrationAsync(connection, registration);
    }

    [Tracing]
    private async Task InsertRegistrationAsync(
        NpgsqlConnection connection,
        Registration registration
        )
    {
        await connection.ExecuteAsync(
            """
            INSERT INTO ecommerce.registrations (Email, FirstName, LastName, AddressLine1, AddressLine2, City, State, Zip, RegistrationDate, BookTitle)
            VALUES (@Email, @FirstName, @LastName, @AddressLine1, @AddressLine2, @City, @State, @Zip, @RegistrationDate, @BookTitle);
            """,
            new
            {
                registration.Email,
                registration.FirstName,
                registration.LastName,
                registration.AddressLine1,
                registration.AddressLine2,
                registration.City,
                registration.State,
                registration.Zip,
                registration.RegistrationDate,
                registration.BookTitle
            });
    }

    [Tracing(CaptureMode = TracingCaptureMode.Disabled)]
    private async Task<NpgsqlConnection> GetConnectionAsync()
    {
        var credentials = await GetDbCredentialsAsync();

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = EnvReader.GetStringValue("DB_HOST"),
            Port = EnvReader.GetIntValue("DB_PORT"),
            Database = EnvReader.GetStringValue("DB_NAME"),
            Username = credentials!.Username,
            Password = credentials.Password,
            SslMode = EnvReader.GetBooleanValue("DB_REQUIRE_SSL") ? SslMode.Require : SslMode.Allow,
            Pooling = true
        };
        return new NpgsqlConnection(connectionString.ConnectionString);
    }

    [Tracing(CaptureMode = TracingCaptureMode.Disabled)]
    private async Task<DbCredentials?> GetDbCredentialsAsync()
    {
        var credentialsJson = await ParametersManager
            .SecretsProvider
            .GetAsync(EnvReader.GetStringValue("DB_CREDENTIALS_SECRET_NAME"));
        return JsonSerializer.Deserialize<DbCredentials>(credentialsJson!, GetJsonOptions());
    }

    [Tracing]
    private static JsonSerializerOptions GetJsonOptions() =>
        new()
        {
            PropertyNameCaseInsensitive = true,
        };
}