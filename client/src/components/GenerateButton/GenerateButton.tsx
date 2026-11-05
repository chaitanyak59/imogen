interface GenerateButtonProps {
    isGenerating: boolean;
    disabled: boolean;
}

function GenerateButton({ isGenerating, disabled }: GenerateButtonProps) {
    return (
        <button type="submit" disabled={disabled || isGenerating}>
            {isGenerating ? "Generating..." : "Generate Gift Card"}
        </button>
    );
}

export default GenerateButton;