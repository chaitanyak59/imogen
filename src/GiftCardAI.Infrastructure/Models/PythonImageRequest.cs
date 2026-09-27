using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Infrastructure.Models;

public sealed record PythonGenerateImageRequest(
    string Prompt,
    int Steps,
    int Seed,
    int Width,
    int Height);
