import { useState, type ChangeEvent } from "react";
import "./App.css";
import { generateImage } from "./api/imageApi";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

function App() {
  const [prompt, setPrompt] = useState("");
  const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [isGenerating, setIsGenerating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(event: ChangeEvent) {
    event.preventDefault();

    if (!prompt.trim()) {
      return;
    }

    setIsGenerating(true);
    setError(null);

    try {
      const imageBlob = await generateImage({ prompt });
      const newImageUrl = URL.createObjectURL(imageBlob);

      setImageUrl((previousImageUrl) => {
        if (previousImageUrl) {
          URL.revokeObjectURL(previousImageUrl);
        }

        return newImageUrl;
      });
    } catch (exception) {
      console.error(exception);

      setError("Image generation failed.");
    } finally {
      setIsGenerating(false);
    }
  }

  return (
    <main>
      <h1>CHAY IMAGE GEN</h1>

      <form onSubmit={handleSubmit}>
        <label htmlFor="prompt">
          Describe your gift card
        </label>

        <textarea
          id="prompt"
          value={prompt}
          onChange={(event) => setPrompt(event.target.value)}
          placeholder="A premium birthday gift card with balloons and confetti..."
          rows={5}
        />

        <button type="submit" disabled={isGenerating}>
          {isGenerating ? "Generating..." : "Generate"}
        </button>
      </form>

      {error && <p>{error}</p>}

      {imageUrl && (
        <div>
          <h2>Generated Image</h2>

          <img src={imageUrl} alt="AI generated gift card" />
        </div>
      )}
    </main>
  );
}

export default App;