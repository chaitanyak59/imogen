using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GiftCardAI.Infrastructure.Image.Bedrock;

public sealed record BedrockImageRequest
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "text-to-image";

    [JsonPropertyName("aspect_ratio")]
    public string AspectRatio { get; set; } = "1:1";

    [JsonPropertyName("output_format")]
    public string OutputFormat { get; set; } = "jpeg";

    [JsonPropertyName("seed")]
    public long Seed { get; set; }
    [JsonPropertyName("negative_prompt")]
    public string? NegativePrompt { get; set; }
}
