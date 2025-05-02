using Microsoft.Extensions.Logging;
using PwrTlzDemo.Providers;

namespace PwrTlzDemo.Services;

internal class ProductsService
{
    private readonly EcommerceDataProvider _ecommerceDataProvider;

    private readonly ILogger<ProductsService> _logger;

    public ProductsService(EcommerceDataProvider ecommerceDataProvider, ILogger<ProductsService> logger)
    {
        _ecommerceDataProvider =
            ecommerceDataProvider ?? throw new ArgumentNullException(nameof(ecommerceDataProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExecuteAsync()
    {
        var products = await _ecommerceDataProvider.GetProductsAsync();

        foreach (var product in products)
            _logger.LogInformation("Product: {Product}", product);

    }
}