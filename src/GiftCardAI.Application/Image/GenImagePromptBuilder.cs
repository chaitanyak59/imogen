using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public class GenImagePromptBuilder
{
    private const string MasterPrompt = """
        Create premium digital gift card artwork for the occasion: {0}, with a {1} visual theme.

        Use a clean, modern commercial graphic design style with beautiful illustrations, appropriate colors, decorative elements and balanced composition.

        Create a flat, front-facing 2D landscape design with full-bleed artwork and a visually balanced central area reserved for text.

        Do not generate any text, letters, words, numbers, typography, captions, logos or watermarks.

        No physical card mockups, perspective, shadows, surrounding environments or 3D effects.
    """;

    private static readonly Dictionary<string, string> OccasionHeadings = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Thank You"] = "Thank You",
        ["Birthday"] = "Happy Birthday",
        ["Christmas"] = "Merry Christmas",
        ["Wedding"] = "Congratulations",
        ["Anniversary"] = "Happy Anniversary",
        ["Graduation"] = "Congratulations",
        ["Valentine's Day"] = "Happy Valentine's Day",
        ["New Year"] = "Happy New Year"
    };

    public string GetHeading(string occasion)
    {
        return OccasionHeadings.TryGetValue(occasion, out var heading)
            ? heading
            : occasion;
    }

    public string Build(string occasion, string theme, string message = "")
    {
        return string.Format(
        CultureInfo.InvariantCulture,
        MasterPrompt,
        GetHeading(occasion),
        theme,
        string.IsNullOrWhiteSpace(message) ? "No custom message" : message.Trim());
    }
}
