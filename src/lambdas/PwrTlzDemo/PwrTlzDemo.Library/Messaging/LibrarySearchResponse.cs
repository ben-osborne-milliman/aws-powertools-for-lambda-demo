using System.Text.Json.Serialization;

namespace PwrTlzDemo.Library.Messaging;

public record LibrarySearchResponse
{
    public int? NumFound { get; init; }

    public int? Start { get; init; }

    public bool? NumFoundExact { get; init; }

    [JsonPropertyName("documentation_url")]
    public string? DocumentationUrl { get; init; }

    public string? Q { get; init; }

    public int? Offset { get; init; }

    public List<Doc>? Docs { get; init; }
}

public record Doc
{
    [JsonPropertyName("author_key")]
    public List<string>? AuthorKey { get; init; }

    [JsonPropertyName("author_name")]
    public List<string>? AuthorName { get; init; }

    [JsonPropertyName("cover_edition_key")]
    public string? CoverEditionKey { get; init; }

    [JsonPropertyName("cover_i")]
    public int? CoverId { get; init; }

    [JsonPropertyName("ebook_access")]
    public string? EbookAccess { get; init; }

    [JsonPropertyName("edition_count")]
    public int? EditionCount { get; init; }

    [JsonPropertyName("first_publish_year")]
    public int? FirstPublishYear { get; init; }

    [JsonPropertyName("has_fulltext")]
    public bool? HasFullText { get; init; }

    [JsonPropertyName("ia")]
    public List<string>? Ia { get; init; }

    [JsonPropertyName("ia_collection_s")]
    public string? IaCollectionS { get; init; }

    public string? Key { get; init; }

    public List<string>? Language { get; init; }

    [JsonPropertyName("lending_edition_s")]
    public string? LendingEditionS { get; init; }

    [JsonPropertyName("lending_identifier_s")]
    public string? LendingIdentifierS { get; init; }

    [JsonPropertyName("public_scan_b")]
    public bool? PublicScanB { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }
}