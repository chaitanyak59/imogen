export interface GenerateImageRequest {
    prompt: string;
}

export async function generateImage(request: GenerateImageRequest): Promise<Blob> {
    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;
    const seed = Math.floor(Math.random() * 2_147_483_647);

    const response = await fetch(`${apiBaseUrl}/api/imagegen/generate`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) {
        throw new Error(`Image generation failed with status ${response.status}.`);
    }

    return response.blob();
}