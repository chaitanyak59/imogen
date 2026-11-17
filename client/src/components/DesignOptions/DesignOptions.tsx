import type { DesignOption } from "../../types/image";
import "./DesignOptions.css";

interface DesignOptionsProps {
    occasion: string;
    theme: string;
    message: string;
    occasions: DesignOption[];
    themes: DesignOption[];
    onOccasionChange: (value: string) => void;
    onThemeChange: (value: string) => void;
    onMessageChange: (value: string) => void;
}

function DesignOptions({
    occasion,
    theme,
    occasions,
    themes,
    message,
    onOccasionChange,
    onThemeChange,
    onMessageChange
}: DesignOptionsProps) {
    return (
        <div className="design-options">
            <div className="design-option">
                <label htmlFor="occasion">Occasion</label>
                <select id="occasion" value={occasion} onChange={(event) => onOccasionChange(event.target.value)}>
                    {occasions.map((option) => (
                        <option key={option.value} value={option.value}>
                            {option.label}
                        </option>
                    ))}
                </select>
            </div>

            <div className="design-option">
                <label htmlFor="theme">Theme</label>
                <select id="theme" value={theme} onChange={(event) => onThemeChange(event.target.value)}>
                    {themes.map((option) => (
                        <option key={option.value} value={option.value}>
                            {option.label}
                        </option>
                    ))}
                </select>
            </div>
            <div className="design-option design-option--message">
                <label htmlFor="message">Gift Card Message</label>
                <textarea
                    id="message"
                    onChange={(event) => onMessageChange(event.target.value)}
                    value={message}
                    maxLength={100}
                />
            </div>
        </div>
    );
}

export default DesignOptions;