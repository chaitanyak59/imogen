# ChayGiftCardGen

ChayGiftCardGen is a local AI-powered gift card image generation application.

A user enters a description in the React UI, the request is sent through the .NET API to a local Python AI service, and FLUX.2 generates the image using Apple Silicon through MLX/MFLUX.

The project is currently designed as a local development and learning project, while keeping the architecture suitable for eventually replacing the local AI implementation with a hosted provider such as AWS Bedrock.

## Architecture

```text
┌──────────────────────────────┐
│         React + Vite         │
│      http://localhost:5173   │
└──────────────┬───────────────┘
               │
               │ HTTPS
               │ POST /api/images/generate
               ▼
┌──────────────────────────────┐
│        ASP.NET Core API      │
│     https://localhost:5001   │
│                              │
│  Validation                  │
│  Application contracts       │
│  AI abstraction              │
│  Error handling              │
└──────────────┬───────────────┘
               │
               │ HTTP
               │ POST /images/generate
               ▼
┌──────────────────────────────┐
│        Python FastAPI        │
│      http://127.0.0.1:5005   │
│                              │
│  Persistent FLUX model       │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         MFLUX / MLX          │
│                              │
│  FLUX.2 Klein 4B - 4 bit     │
│  Apple Silicon inference     │
└──────────────┬───────────────┘
               │
               ▼
          Generated PNG
```

The generated PNG travels back through the same path:

```text
FLUX
  ↓
Python FastAPI
  ↓
.NET API
  ↓
React
  ↓
Browser displays image
```

## Project Structure

```text
ChayGiftCardGen/
│
├── ChayGiftCardGen.sln
│
├── src/
│   ├── ChayGiftCardGen.Api/
│   │   ├── Controllers/
│   │   ├── Properties/
│   │   ├── Program.cs
│   │   └── appsettings.Development.json
│   │
│   ├── ChayGiftCardGen.Application/
│   │   └── ImageGeneration/
│   │       ├── IImageGenerationService.cs
│   │       └── GenerateImageRequest.cs
│   │
│   ├── ChayGiftCardGen.Domain/
│   │
│   └── ChayGiftCardGen.Infrastructure/
│       └── ImageGeneration/
│           ├── PythonImageGenerationService.cs
│           └── PythonGenerateImageRequest.cs
│
├── tests/
│   └── ChayGiftCardGen.Api.Tests/
│
├── client/
│   ├── src/
│   │   ├── api/
│   │   │   └── imageApi.ts
│   │   └── App.tsx
│   └── .env.development
│
├── ai/
│   ├── .venv/
│   ├── outputs/
│   ├── server.py
│   └── requirements.txt
│
├── .gitignore
└── README.md
```

## Layer Responsibilities

### React Client

The React application is responsible for the user experience.

It:

- Accepts the user's gift card description.
- Calls the .NET API.
- Receives the generated image as binary data.
- Converts the response to a browser Blob.
- Displays the generated image.

React does not communicate directly with Python or FLUX.

```text
React → .NET → Python → FLUX
```

This keeps the AI implementation hidden from the browser.

### ChayGiftCardGen.Api

The ASP.NET Core API is the public backend entry point.

It is responsible for:

- HTTP endpoints.
- Request validation.
- CORS.
- Dependency injection.
- Configuration.
- Exception handling.
- Returning generated images to the client.

The API should not contain FLUX- or MFLUX-specific implementation logic.

### ChayGiftCardGen.Application

The Application project defines what the application can do.

For example:

```text
IImageGenerationService
GenerateImageRequest
```

The public application request currently contains only information the consumer should control:

```json
{
  "prompt": "A premium birthday gift card with balloons"
}
```

AI implementation settings such as model inference steps and seed are intentionally not exposed to the browser.

### ChayGiftCardGen.Domain

The Domain project is reserved for core business concepts.

As the project grows, this can contain concepts such as:

```text
DesignSession
GeneratedDesign
DesignVersion
GenerationStatus
```

The Domain layer should not know about:

```text
React
HTTP
FastAPI
Python
MFLUX
FLUX
SQL Server
AWS Bedrock
```

### ChayGiftCardGen.Infrastructure

Infrastructure implements external integrations.

Currently:

```text
IImageGenerationService
        ↓
PythonImageGenerationService
```

The Infrastructure layer converts the application's request into the internal Python AI request.

For example:

```text
Application request

{
    Prompt
}

        ↓

Infrastructure

        ↓

Python request

{
    Prompt,
    Steps,
    Seed,
    Width,
    Height
}
```

This prevents AI implementation details from leaking into the public API.

### Python AI Service

FastAPI provides the internal AI inference API.

Its responsibility is:

```text
HTTP request
    ↓
Validate AI request
    ↓
Run FLUX inference
    ↓
Generate PNG
    ↓
Return PNG
```

The FLUX model is loaded once when the Python service starts and remains in memory.

This avoids loading the multi-GB model for every image request.

## AI Model

The current model is:

```text
mlx-community/flux2-klein-4b-4bit
```

It runs locally using:

```text
MFLUX
  ↓
MLX
  ↓
Apple Silicon GPU / Unified Memory
```

The 4-bit model is used to reduce memory requirements and make local inference practical.

The current development machine is an Apple Silicon Mac with 16 GB unified memory.

## Image Generation Settings

The internal generation request currently uses settings similar to:

```text
Steps  = 4
Seed   = randomly generated
Width  = 1024
Height = 720
```

### Prompt

The prompt describes what should be generated.

Example:

```text
A premium birthday gift card with colorful balloons,
elegant typography and a dark blue background
```

### Seed

The seed determines the random starting noise used by the image model.

Different seeds can create different designs from the same prompt.

```text
Same Prompt + Seed 100 → Design A

Same Prompt + Seed 200 → Design B

Same Prompt + Seed 300 → Design C
```

The seed does not represent image quality or generation effort.

.NET currently generates it using:

```csharp
Random.Shared.Next()
```

### Steps

Steps control how many inference/refinement steps the model performs.

FLUX.2 Klein is designed for very low-step generation.

The current default is:

```text
4 steps
```

Increasing the seed does not increase generation time.

Increasing inference steps can increase generation time.

## Prerequisites

The following are required for local development.

### macOS / Apple Silicon

The current local AI implementation uses MLX and is intended for Apple Silicon Macs.

For example:

```text
M1
M2
M3
M4
```

The current development environment uses an M4 Mac Mini with 16 GB unified memory.

### .NET SDK

Install a compatible .NET SDK.

The project currently targets:

```text
.NET 10
```

Verify:

```bash
dotnet --version
```

### Node.js and npm

Required for the React/Vite application.

Verify:

```bash
node --version
npm --version
```

### Python

The AI service currently uses:

```text
Python 3.12
```

Verify:

```bash
python3 --version
```

### Hugging Face Model

The model is downloaded from Hugging Face:

```text
mlx-community/flux2-klein-4b-4bit
```

Hugging Face normally stores downloaded model files under:

```text
~/.cache/huggingface/
```

The model does not need to be downloaded every time the application starts.

MFLUX will reuse the cached model when available.

## Initial Setup

### 1. Clone/Open the Repository

Move to the repository:

```bash
cd ChayGiftCardGen
```

### 2. Restore .NET Dependencies

From the repository root:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

### 3. Configure the Python Environment

Move to the AI project:

```bash
cd ai
```

Create the virtual environment if it does not already exist:

```bash
python3.12 -m venv .venv
```

Activate it:

```bash
source .venv/bin/activate
```

Install Python dependencies:

```bash
pip install -r requirements.txt
```

The important dependencies include:

```text
mflux
fastapi
uvicorn
```

The `.venv` directory should not be committed to Git.

### 4. Configure React

Move to:

```bash
cd client
```

Install dependencies:

```bash
npm install
```

The development environment configuration should contain:

```text
VITE_API_BASE_URL=https://localhost:5001
```

For example:

```text
client/.env.development
```

### 5. Trust the .NET Development Certificate

The React application communicates with the .NET API using HTTPS.

Check the development certificate:

```bash
dotnet dev-certs https --check --trust
```

If necessary:

```bash
dotnet dev-certs https --trust
```

## Starting the Application

Three processes need to run during local development.

It is easiest to use three terminal windows.

### Terminal 1 — Python AI Service

From the repository root:

```bash
cd ai
source .venv/bin/activate
uvicorn server:app --host 127.0.0.1 --port 5005
```

The service should start at:

```text
http://127.0.0.1:5005
```

Health endpoint:

```text
GET http://127.0.0.1:5005/health
```

The first startup can take longer because the FLUX model needs to be loaded into memory.

Do not use Uvicorn `--reload` for normal image-generation testing because automatic reloads can cause the large model to be loaded repeatedly.

### Terminal 2 — .NET API

From the repository root:

```bash
dotnet run --project src/ChayGiftCardGen.Api --launch-profile https
```

The API should run at approximately:

```text
https://localhost:5001
```

The exact configured URLs are defined in:

```text
src/ChayGiftCardGen.Api/Properties/launchSettings.json
```

The .NET API communicates internally with Python at:

```text
http://127.0.0.1:5005
```

### Terminal 3 — React

From the repository root:

```bash
cd client
npm run dev
```

Vite should start the application at:

```text
http://localhost:5173
```

Open that address in the browser.

## Complete Startup Order

The recommended order is:

```text
1. Python AI service
       ↓
2. .NET API
       ↓
3. React
```

Or:

```text
Terminal 1

cd ai
source .venv/bin/activate
uvicorn server:app --host 127.0.0.1 --port 5005


Terminal 2

dotnet run --project src/ChayGiftCardGen.Api --launch-profile https


Terminal 3

cd client
npm run dev
```

Then open:

```text
http://localhost:5173
```

## Request Flow

When the user clicks Generate:

```text
1. User enters prompt

        ↓

2. React

POST /api/images/generate

{
    "prompt": "A premium birthday gift card..."
}

        ↓

3. .NET validates the request

        ↓

4. Infrastructure creates internal AI settings

Prompt
Steps = 4
Random Seed
Width = 1024
Height = 720

        ↓

5. .NET calls Python

POST /images/generate

        ↓

6. FastAPI uses the already-loaded FLUX model

        ↓

7. FLUX generates the image

        ↓

8. Python returns image/png

        ↓

9. .NET returns image/png

        ↓

10. React receives response.blob()

        ↓

11. Browser creates an object URL

        ↓

12. <img> displays the generated image
```

## Why Python Is Behind .NET

React intentionally does not call FastAPI directly.

We use:

```text
React
   ↓
.NET
   ↓
Python
```

instead of:

```text
React
   ↓
Python
```

because .NET acts as the application and security boundary.

Eventually it can control:

```text
Authentication
Authorization
Prompt validation
Prompt construction
Rate limiting
Usage limits
Model selection
Generation settings
Logging
Auditing
Cost controls
Provider selection
```

The browser should not be able to freely control the underlying AI model.

## Why MFLUX Is Behind an Interface

The application depends on:

```text
IImageGenerationService
```

rather than directly depending on MFLUX.

Today:

```text
IImageGenerationService
        ↓
PythonImageGenerationService
        ↓
MFLUX
```

In the future it could become:

```text
IImageGenerationService
        ↓
BedrockImageGenerationService
        ↓
AWS Bedrock
```

or another provider.

The application layer does not need to change simply because the AI provider changes.

This is important for keeping the AI layer provider-agnostic.

## Error Handling

ASP.NET Core uses centralized exception handling and ProblemDetails.

The intention is:

```text
Python / FLUX failure
        ↓
Infrastructure
        ↓
ASP.NET exception handling
        ↓
Safe HTTP error
        ↓
React
```

Internal details such as:

```text
Python stack traces
Local filesystem paths
Internal service URLs
Model implementation details
```

should not be exposed to the browser.

## Current Scope

The current implementation supports:

```text
Text prompt
    ↓
Generate new image
    ↓
Return PNG
    ↓
Display image
```

It is currently a text-to-image generation prototype.

## Planned Architecture

The next major feature is to introduce design sessions and image versions.

Instead of independent generations:

```text
Prompt
  ↓
Image
```

the application will move toward:

```text
Design Session

Initial Prompt
    ↓
Version 1

"Make the background darker"
    ↓
Version 2

"Add more balloons"
    ↓
Version 3
```

This will introduce concepts such as:

```text
DesignSession
DesignVersion
Original prompt
Edit instructions
Seed
Generation settings
Image location
Created date
Generation status
```

Image files should eventually live in object storage rather than SQL.

For example, production could use:

```text
SQL Server
    ↓
Metadata

Amazon S3
    ↓
Generated images
```

## Long-Term Direction

The project is intended to evolve from:

```text
Local Gift Card Image Generator
```

into a reusable:

```text
eCommerce AI Layer
```

Potential capabilities include:

```text
Gift card image generation
Image editing
Product imagery
Marketing content
Product descriptions
Personalization
AI-assisted design
Other controlled eCommerce AI tools
```

The architecture should continue following one important principle:

```text
Application
    ↓
AI abstraction
    ↓
Provider implementation
```

rather than coupling the application directly to a specific model or AI provider.