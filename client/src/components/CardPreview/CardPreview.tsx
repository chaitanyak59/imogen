import "./CardPreview.css";

interface CardPreviewProps {
    imageUrl: string | null;
    isGenerating: boolean;
}

function CardPreview({ imageUrl, isGenerating }: CardPreviewProps) {
    return (
        <div className="gift-card-preview">
            {isGenerating ? (
                <div className="preview-placeholder">
                    <div className="loader" />
                    <p>Creating your gift card...</p>
                </div>
            ) : imageUrl ? (
                <img src={imageUrl} alt="Generated gift card" />
            ) : (
                <div className="preview-placeholder">
                    <span className="preview-icon">🎁</span>
                    <h2>Your gift card will appear here</h2>
                    <p>Choose an occasion and theme to get started.</p>
                </div>
            )}
        </div>
    );
}

export default CardPreview;