using System.Text.Json;

namespace PCBack.AI;

/// <summary>
/// Client for calling an LLM API.
/// </summary>
public interface IAiClient
{
    Task<string> GenerateAsync(string prompt);
}

/// <summary>
/// OpenAI chat completions API implementation.
/// </summary>
public class AiClient : IAiClient
{
    private const string ChatCompletionsPath = "v1/chat/completions";

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private static readonly JsonSerializerOptions RequestJsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private static readonly JsonSerializerOptions ResponseJsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateAsync(string prompt)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return string.Empty;

        var request = new OpenAiChatRequest
        {
            Model = "gpt-4o-mini",
            Messages = new List<OpenAiMessage> { new() { Role = "user", Content = prompt } },
            Temperature = 0.2
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, ChatCompletionsPath);
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        httpRequest.Content = JsonContent.Create(request, options: RequestJsonOptions);

        using var response = await _httpClient.SendAsync(httpRequest);

        if (!response.IsSuccessStatusCode)
            return string.Empty;

        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<OpenAiChatResponse>(json, ResponseJsonOptions);

        var content = apiResponse?.Choices?.FirstOrDefault()?.Message?.Content;
        return content?.Trim() ?? string.Empty;
    }
}
