using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GiftCardAI.Application.Image;
using GiftCardAI.Infrastructure.Image.Bedrock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChayGiftCardGen.Infrastructure.ImageGeneration.Bedrock;

public sealed class BedrockImageClient
{
    private readonly IConfiguration _configuration;
    private readonly GenImagePromptBuilder _promptBuilder;
    private readonly HttpClient _httpClient;
    private readonly ILogger<BedrockImageClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BedrockImageClient(
        IConfiguration configuration,
        GenImagePromptBuilder promptBuilder,
        HttpClient httpClient,
        ILogger<BedrockImageClient> logger)
    {
        _configuration = configuration;
        _promptBuilder = promptBuilder;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<GenImageResponse> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        if (prompt.Length > 10000)
        {
            throw new ArgumentException("Prompt cannot exceed 10000 characters.", nameof(prompt));
        }

        var modelId = _configuration["AWS:ModelId"];
        var apiKey = _configuration["AWS:ShortKey"];

        if (string.IsNullOrWhiteSpace(modelId) || string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Bedrock configuration is missing.");
        }

        var seed = (long)Random.Shared.NextInt64(0, (long)uint.MaxValue + 1);

        var bedrockRequest = new BedrockImageRequest
        {
            Prompt = prompt,
            Mode = "text-to-image",
            AspectRatio = "3:2",
            OutputFormat = "png",
            Seed = seed,
            NegativePrompt = "photograph of a physical card, card mockup, card held by hands, card on a table, grey background, surrounding environment, perspective view, 3D render, drop shadow, floating card, realistic product photography, gibberish text, random letters, fake logos, watermarks"
        };

        var json = JsonSerializer.Serialize(bedrockRequest);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"model/{modelId}/invoke");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = new StringContent(json, Encoding.UTF8);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Bedrock invocation failed ({(int)response.StatusCode}): {error}", null, response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<BedrockImageResponse>(JsonOptions, cancellationToken);

        if (result?.Images is not { Count: > 0 })
        {
            throw new InvalidOperationException("Bedrock returned no images.");
        }

        var imageBytes = Convert.FromBase64String(result.Images[0]);
        var actualSeed = result.Seeds?.FirstOrDefault() ?? seed;

        return new GenImageResponse(imageBytes, actualSeed);
    }
}