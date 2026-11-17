using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GiftCardAI.Infrastructure.Image.Bedrock;

public sealed record BedrockImageResponse
{
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];

    [JsonPropertyName("seeds")]
    public List<uint> Seeds { get; set; } = [];

    [JsonPropertyName("finish_reasons")]
    public List<string?> FinishReasons { get; set; } = [];
}