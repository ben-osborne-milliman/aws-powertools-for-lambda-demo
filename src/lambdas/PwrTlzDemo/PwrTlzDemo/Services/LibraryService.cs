using AWS.Lambda.Powertools.Tracing;
using System.Text.Json;
using System.Text.Json.Serialization;
using PwrTlzDemo.Messaging;

namespace PwrTlzDemo.Services;

internal class LibraryService
{
    private readonly IHttpClientFactory _clientFactory;

    public LibraryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    [Tracing]
    public async Task<LibrarySearchResponse> GetBooksAsync()
    {
        var random = new Random();
        var randomQuery = $"{(char)random.Next('a', 'z' + 1)}{(char)random.Next('a', 'z' + 1)}";

        using var httpClient  = _clientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, $"http://openlibrary.org/search.json?q={randomQuery}&limit=5");
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<LibrarySearchResponse>(responseContent, GetJsonOptions());
        return result!;
    }

    [Tracing]
    private static JsonSerializerOptions GetJsonOptions() =>
        new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };
}