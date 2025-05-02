using Dapper;
using dotenv.net.Utilities;
using IntelliScript.RdsPgConnector.Abstractions;
using IntelliScript.RdsPgConnector.Models;
using PwrTlzDemo.Models;

namespace PwrTlzDemo.Providers;

internal class EcommerceDataProvider
{
    private readonly IConnectionFactory _connectionFactory;

    public EcommerceDataProvider(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var connectionResponse = await _connectionFactory
            .GetConnectionAsync(new ConnectionRequest
            {
                Host = EnvReader.GetStringValue("DB_HOST"),
                Port = EnvReader.GetIntValue("DB_PORT"),
                Database = EnvReader.GetStringValue("DB_NAME"),
                Username = EnvReader.GetStringValue("DB_USER"),
                Password = EnvReader.GetStringValue("DB_PASSWORD"),
                RequireSsl = EnvReader.GetBooleanValue("DB_REQUIRE_SSL"),
                UseIamAuth = EnvReader.GetBooleanValue("USE_IAM_AUTH"),
                Pooling = true
            }, CancellationToken.None);

        if(!connectionResponse.Success)
            throw new AggregateException(string.Join(Environment.NewLine, connectionResponse.Errors));

        await using var connection = connectionResponse.Connection!;

        return await connection.QueryAsync<Product>("SELECT * FROM ecommerce.products;");
    }
}