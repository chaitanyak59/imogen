### How to start the app

``` 
source .venv/bin/activate
uvicorn server:app --host 127.0.0.1 --port 5689

TEST:
curl -X POST http://localhost:5689/images/generate -H "Content-Type: application/json" -d '{"prompt":"A premium futuristic birthday gift card with balloons and confetti, elegant ecommerce illustration, no text","steps":4,"seed":42}' --output generated.png

DOTNET TEST:
curl -X POST http://localhost:5688/api/images/generate -H "Content-Type: application/json" -d '{"prompt":"A premium birthday gift card with balloons, elegant modern design, no text","steps":4,"seed":42,"width":1024,"height":720}' --output dotnet-generated.png

```