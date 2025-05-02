using AWS.Lambda.Powertools.Tracing;
using Microsoft.Extensions.Logging;
using PwrTlzDemo.Models;
using PwrTlzDemo.Providers;

namespace PwrTlzDemo.Services;

internal class ProductsService
{
    private readonly EcommerceDataProvider _ecommerceDataProvider;

    public ProductsService(EcommerceDataProvider ecommerceDataProvider, ILogger<ProductsService> logger)
    {
        _ecommerceDataProvider =
            ecommerceDataProvider ?? throw new ArgumentNullException(nameof(ecommerceDataProvider));
    }

    [Tracing]
    public async Task<IEnumerable<Product>> ExecuteAsync()
    {
        var products = await _ecommerceDataProvider.GetProductsAsync();
        return products;
    }
}