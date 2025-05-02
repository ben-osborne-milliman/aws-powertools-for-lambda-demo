using AWS.Lambda.Powertools.Tracing;
using PwrTlzDemo.Messaging;

namespace PwrTlzDemo.Services;

internal class HandlerService
{
    private readonly ProductsService _productsService;

    private readonly LibraryService _libraryService;

    public HandlerService(ProductsService productsService, LibraryService libraryService)
    {
        _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
        _libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
    }

    [Tracing]
    public async Task<InventoryResponse> ExecuteAsync()
    {
        var products = await _productsService.GetProductsAsync();
        var librarySearchResponse = await _libraryService.GetBooksAsync();

        return new InventoryResponse
        {
            Products = products.Take(5).ToList(),
            Books = librarySearchResponse.Docs
        };
    }
}