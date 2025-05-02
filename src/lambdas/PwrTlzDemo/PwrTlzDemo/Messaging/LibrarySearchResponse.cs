using System.Text.Json.Serialization;

namespace PwrTlzDemo.Messaging;

internal record LibrarySearchResponse
{
    public required int NumFound { get; init; }

    public required int Start { get; init; }

    public required bool NumFoundExact { get; init; }

    [JsonPropertyName("documentation_url")]
    public required string DocumentationUrl { get; init; }

    public required string Q { get; init; }

    public required int? Offset { get; init; }

    public required List<Doc> Docs { get; init; }
}

public record Doc
{
    [JsonPropertyName("author_key")]
    public required List<string> AuthorKey { get; init; }

    [JsonPropertyName("author_name")]
    public required List<string> AuthorName { get; init; }

    [JsonPropertyName("cover_edition_key")]
    public required string CoverEditionKey { get; init; }

    [JsonPropertyName("cover_i")]
    public required int? CoverId { get; init; }

    [JsonPropertyName("ebook_access")]
    public required string EbookAccess { get; init; }

    [JsonPropertyName("edition_count")]
    public required int? EditionCount { get; init; }

    [JsonPropertyName("first_publish_year")]
    public required int? FirstPublishYear { get; init; }

    [JsonPropertyName("has_fulltext")]
    public required bool HasFullText { get; init; }

    [JsonPropertyName("ia")]
    public required List<string> Ia { get; init; }

    [JsonPropertyName("ia_collection_s")]
    public required string? IaCollectionS { get; init; }

    public required string Key { get; init; }

    public required List<string> Language { get; init; }

    [JsonPropertyName("lending_edition_s")]
    public required string? LendingEditionS { get; init; }

    [JsonPropertyName("lending_identifier_s")]
    public required string? LendingIdentifierS { get; init; }

    [JsonPropertyName("public_scan_b")]
    public required bool PublicScanB { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }
}