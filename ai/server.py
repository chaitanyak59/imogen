from fastapi import FastAPI, HTTPException
from fastapi.responses import FileResponse
from pydantic import BaseModel, Field
from mflux.models.common.config import ModelConfig
from mflux.models.flux2.variants import Flux2Klein
from pathlib import Path
from uuid import uuid4

app = FastAPI(
    title="Chay Gift Card AI Image Service",
    version="1.0.0"
)

OUTPUT_DIRECTORY = Path("outputs")

OUTPUT_DIRECTORY.mkdir(exist_ok=True)

print("Loading FLUX.2 model...")

model = Flux2Klein(
    model_config=ModelConfig.from_name(
        model_name="mlx-community/flux2-klein-4b-4bit"
    )
)

print("FLUX.2 model loaded.")

class GenerateImageRequest(BaseModel):
    prompt: str = Field(min_length=1, max_length=1000)
    steps: int = Field(default=4, ge=1, le=20)
    seed: int = Field(default=42, ge=0)
    width: int = Field(default=1024, ge=256, le=1536)
    height: int = Field(default=720, ge=256, le=1536)

@app.get("/health")
async def health():
    return {"status": "healthy"}

@app.post("/images/generate")
async def generate_image(request: GenerateImageRequest):
    image_id = str(uuid4())

    output_path = OUTPUT_DIRECTORY / f"{image_id}.png"

    try:
        image = model.generate_image(
            seed=request.seed,
            prompt=request.prompt,
            num_inference_steps=request.steps,
            width=request.width,
            height=request.height
        )

        image.save(str(output_path))
    except Exception as exception:
        print(f"Image generation failed: {exception}")

        raise HTTPException(
            status_code=500,
            detail="Image generation failed."
        )

    return FileResponse(
        path=output_path,
        media_type="image/png",
        filename=f"{image_id}.png"
    )