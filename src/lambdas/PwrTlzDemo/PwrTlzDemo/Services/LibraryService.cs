using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PwrTlzDemo.Services;

internal class LibraryService
{
    private readonly IHttpClientFactory _clientFactory;

    public LibraryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
    }

    [Tracing]
    [Logging(Service = "LibraryService")]
    public async Task<LibrarySearchResponse> GetBooksAsync()
    {
        var random = new Random();
        var randomQuery = $"{(char)random.Next('a', 'z' + 1)}{(char)random.Next('a', 'z' + 1)}";

        Logger.LogInformation($"GetBooks request: {randomQuery}");

        var stopwatch = Stopwatch.StartNew();

        using var httpClient  = _clientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, $"http://openlibrary.org/search.json?q={randomQuery}&limit=5");
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        stopwatch.Stop();
        Metrics.AddMetric("GetBooksDuration", stopwatch.ElapsedMilliseconds, MetricUnit.Milliseconds);

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