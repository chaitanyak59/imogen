namespace GiftCardAI.Application.Image.Validator;

public sealed class GenImageValidator
{
    private static readonly HashSet<string> SupportedOccasions = new(StringComparer.OrdinalIgnoreCase)
    {
        "birthday",
        "anniversary",
        "wedding",
        "christmas",
        "graduation",
        "thank-you"
    };

    private static readonly HashSet<string> SupportedThemes = new(StringComparer.OrdinalIgnoreCase)
    {
        "elegant",
        "minimalist",
        "luxury",
        "floral",
        "modern",
        "playful"
    };

    public bool IsValid(string occasion, string theme)
    {
        return SupportedOccasions.Contains(occasion) && SupportedThemes.Contains(theme);
    }
}