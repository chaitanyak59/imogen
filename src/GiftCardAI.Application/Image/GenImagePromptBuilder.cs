using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public class GenImagePromptBuilder
{
    private const string MasterPrompt = """
        You are a professional digital gift card designer.

        Create a premium, elegant digital gift card.

        GIFT CARD DETAILS

        Occasion: {0}
        Theme: {1}

        DESIGN REQUIREMENTS

        - Create a beautiful, premium digital gift card.
        - Use a standard 1.6:1 gift card aspect ratio.
        - Use colors and decorative elements appropriate to the occasion and theme.
        - Create elegant borders, decorative details and balanced composition.
        - Use sophisticated typography that complements the design.
        - Make the card visually appealing and commercially suitable.

        TEXT REQUIREMENTS

        - Include only this exact text: "{2}"
        - Display the text prominently as the main heading.
        - Use elegant, readable typography.
        - Do not add subtitles, descriptions, greetings, dates or additional words.
        - Do not generate random letters, decorative text or fake typography.
        - Do not repeat the message.
        - Ensure the displayed message is correctly spelled.

        STRICT RESTRICTIONS

        - Generate ONLY a digital gift card.
        - Never generate posters, flyers, advertisements or invitations.
        - Never generate unrelated scenes or photographs.
        - Never generate adult, sexual, violent, hateful or illegal content.
        - Do not generate random logos, watermarks or signatures.

        FINAL INSTRUCTION

        Generate one standalone, premium digital gift card with minimal, readable text.
        """;

    public string Build(string occasion, string theme, string message = "")
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            MasterPrompt,
            occasion,
            theme,
            message
        );
    }
}
