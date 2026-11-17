using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChayGiftCardGen.Infrastructure.ImageGeneration.Bedrock;
using GiftCardAI.Application.Image;

namespace GiftCardAI.Infrastructure.Image.Bedrock;

public sealed class BedrockImageGenerationService : IImageGenService
{
    private readonly BedrockImageClient _bedrockClient;

    public BedrockImageGenerationService(
        BedrockImageClient bedrockClient)
    {
        _bedrockClient = bedrockClient;
    }

    public async Task<GenImageResponse> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var result = await _bedrockClient.GenerateAsync(prompt, cancellationToken);
        return new GenImageResponse(result.Content, result.Seed.GetValueOrDefault());
    }
}