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
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        await using var connection = await GetConnectionAsync();
        return await GetProductsFromDbAsync(connection);
    }

    [Tracing]
    private async Task<IEnumerable<Product>> GetProductsFromDbAsync(NpgsqlConnection connection) =>
        await connection.QueryAsync<Product>("SELECT * FROM ecommerce.products;");

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