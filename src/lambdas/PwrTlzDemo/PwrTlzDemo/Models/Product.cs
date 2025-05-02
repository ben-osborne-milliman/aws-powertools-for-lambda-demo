namespace PwrTlzDemo.Models;

public record Product
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string? Description { get; init; }

    public required decimal Price { get; init; }

    public required DateTime InsertedOn { get; init; }

    public required DateTime ModifiedOn { get; init; }
}