namespace PwrTlzDemo.Models;

internal record Product
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string? Description { get; init; }

    public required decimal Price { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; init; }
}