import { useEffect, useState } from "react";
import { generateImage } from "../../api/imageApi";
import type { DesignOption } from "../../types/image";
import DesignOptions from "../../components/DesignOptions/DesignOptions";
import GenerateButton from "../../components/GenerateButton/GenerateButton";
import "./GiftCardPage.css";
import CardPreview from "../../components/CardPreview/CardPreview";

const occasions: DesignOption[] = [
    { label: "Birthday", value: "birthday" },
    { label: "Anniversary", value: "anniversary" },
    { label: "Wedding", value: "wedding" },
    { label: "Christmas", value: "christmas" },
    { label: "Thank You", value: "thank-you" },
    { label: "Graduation", value: "graduation" }
];

const themes: DesignOption[] = [
    { label: "Elegant", value: "elegant" },
    { label: "Minimalist", value: "minimalist" },
    { label: "Luxury", value: "luxury" },
    { label: "Floral", value: "floral" },
    { label: "Modern", value: "modern" },
    { label: "Playful", value: "playful" }
];

function GiftCardDesigner() {
    const [occasion, setOccasion] = useState("birthday");
    const [theme, setTheme] = useState("elegant");
    const [message, setMessage] = useState("");
    const [imageUrl, setImageUrl] = useState<string | null>(null);
    const [isGenerating, setIsGenerating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        return () => {
            if (imageUrl) {
                URL.revokeObjectURL(imageUrl);
            }
        };
    }, [imageUrl]);

    async function handleGenerate() {
        setIsGenerating(true);
        setError(null);

        try {
            const imageBlob = await generateImage({ occasion, theme, message });
            const newImageUrl = URL.createObjectURL(imageBlob);

            setImageUrl(newImageUrl);
        } catch (exception) {
            console.error(exception);
            setError("Unable to generate your gift card. Please try again.");
        } finally {
            setIsGenerating(false);
        }
    }

    return (
        <main className="gift-card-designer">
            <header>
                <h1>Chay GiftCard AI</h1>
            </header>

            <CardPreview imageUrl={imageUrl} isGenerating={isGenerating} />

            <form onSubmit={(event) => {
                event.preventDefault();
                void handleGenerate();
            }}>
                <DesignOptions
                    occasion={occasion}
                    theme={theme}
                    occasions={occasions}
                    themes={themes}
                    onOccasionChange={setOccasion}
                    onThemeChange={setTheme}
                    onMessageChange={setMessage}
                />

                <GenerateButton isGenerating={isGenerating} disabled={false} />

                {error && <p className="error-message">{error}</p>}
            </form>
        </main>
    );
}

export default GiftCardDesigner;