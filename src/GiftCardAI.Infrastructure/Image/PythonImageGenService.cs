using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GiftCardAI.Application.Image;
using GiftCardAI.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GiftCardAI.Infrastructure.Image;

public class PythonImageGenService : IImageGenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PythonImageGenService> _logger = null!;


    public PythonImageGenService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<PythonImageGenService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<byte[]> GenerateAsync(GenImageRequest request, CancellationToken cancellationToken = default)
    {
        string? httpClientName = _configuration["AI:ImageHttp"];
        int steps = Convert.ToInt32(_configuration["AI:Image_Steps"]);
        int width = Convert.ToInt32(_configuration["AI:Image_Width"]);
        int height = Convert.ToInt32(_configuration["AI:Image_Height"]);

        HttpClient client = _httpClientFactory.CreateClient(httpClientName ?? "");

        try
        {
            var aiRequest = new PythonGenerateImageRequest(request.Prompt, steps, Random.Shared.Next(), width, height);
            using var response = await client.PostAsJsonAsync("images/generate", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new InvalidOperationException($"AI image generation failed. Status: {(int)response.StatusCode}. Response: {error}");
            }

            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return [];
    }
}
