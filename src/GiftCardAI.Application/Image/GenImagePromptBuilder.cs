using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public class GenImagePromptBuilder
{
    private const string MasterPrompt = """
        Create premium digital gift card artwork for the occasion: {0}, using a {1} visual theme.

        MAIN TEXT REQUIREMENT:
        Display exactly this text as the ONLY greeting highlighter in the entire image: "{2}".

        Make this text the prominent central heading. Preserve the exact spelling, wording and capitalization. Do not modify, repeat, paraphrase or add any words.

        STRICT TEXT RESTRICTIONS:
        Do not generate subtitles, captions, descriptions, secondary headings, greetings, dates, numbers, decorative lettering, random characters or additional text.
        Do not invent text based on the occasion or theme.
        Do not add any text above or below the specified heading.

        DESIGN:
        Use a premium modern commercial graphic design style with beautiful illustrations, appropriate colors and balanced decorative elements.
        Use a flat, front-facing 2D landscape composition with full-bleed artwork.
        Keep the central area clean and uncluttered so the main heading is clearly visible.

        No physical card mockups, perspective, shadows, surrounding environments, 3D effects, logos or watermarks.
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
        var lower = message.ToLower();
        var messageText = char.ToUpper(lower[0]) + lower[1..];

        return string.Format(
        CultureInfo.InvariantCulture,
        MasterPrompt,
        occasion,
        theme,
        messageText);
    }
}
