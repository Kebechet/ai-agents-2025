using System.Text.Json;

namespace AiAgent;

public class SerpApiService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public SerpApiService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public async Task<string> SearchAsync(string query)
    {
        try
        {
            var url = $"https://serpapi.com/search.json?q={Uri.EscapeDataString(query)}&api_key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            // Extract organic results
            if (!json.RootElement.TryGetProperty("organic_results", out var organicResults))
            {
                return "No results found.";
            }

            var results = new System.Text.StringBuilder();
            results.AppendLine($"Search results for: {query}\n");

            var count = 0;
            foreach (var result in organicResults.EnumerateArray())
            {
                if (count >= 5) break;

                var title = result.TryGetProperty("title", out var t) ? t.GetString() : "";
                var snippet = result.TryGetProperty("snippet", out var s) ? s.GetString() : "";
                var link = result.TryGetProperty("link", out var l) ? l.GetString() : "";

                results.AppendLine($"Title: {title}");
                results.AppendLine($"Snippet: {snippet}");
                results.AppendLine($"Link: {link}");
                results.AppendLine();

                count++;
            }

            return results.ToString();
        }
        catch (Exception ex)
        {
            return $"Error searching: {ex.Message}";
        }
    }
}
