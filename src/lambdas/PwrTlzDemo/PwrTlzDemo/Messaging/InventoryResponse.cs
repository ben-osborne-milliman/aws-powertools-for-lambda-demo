using PwrTlzDemo.Models;

namespace PwrTlzDemo.Messaging;

public record InventoryResponse
{
    public required List<Product> Products { get; init; }

    public required List<Doc> Books { get; init; }
}