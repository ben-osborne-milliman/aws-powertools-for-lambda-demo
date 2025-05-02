using AWS.Lambda.Powertools.Tracing;
using Dapper;
using dotenv.net.Utilities;
using IntelliScript.RdsPgConnector.Abstractions;
using IntelliScript.RdsPgConnector.Messaging;
using IntelliScript.RdsPgConnector.Models;
using Npgsql;
using PwrTlzDemo.Models;

namespace PwrTlzDemo.Providers;

internal class EcommerceDataProvider
{
    private readonly IConnectionFactory _connectionFactory;

    public EcommerceDataProvider(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
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
        var connectionResponse = await _connectionFactory
            .GetConnectionAsync(new ConnectionRequest
            {
                Host = EnvReader.GetStringValue("DB_HOST"),
                Port = EnvReader.GetIntValue("DB_PORT"),
                Database = EnvReader.GetStringValue("DB_NAME"),
                Username = EnvReader.GetStringValue("DB_USER"),
                Password = EnvReader.TryGetStringValue("DB_PASSWORD", out var dbPassword) ? dbPassword : string.Empty,
                RequireSsl = EnvReader.GetBooleanValue("DB_REQUIRE_SSL"),
                UseIamAuth = EnvReader.GetBooleanValue("USE_IAM_AUTH"),
                Pooling = true
            }, CancellationToken.None);

        if(!connectionResponse.Success)
            throw new AggregateException(string.Join(Environment.NewLine, connectionResponse.Errors));

        return connectionResponse.Connection!;
    }
}