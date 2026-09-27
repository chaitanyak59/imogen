using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardAI.Application.Image;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageGenController : ControllerBase
{
    private readonly IImageGenService _imageGenSvc;

    public ImageGenController(IImageGenService imageGenSvc)
    {
        _imageGenSvc = imageGenSvc;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(GenImageRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest("Prompt is required.");
        }

        var image = await _imageGenSvc.GenerateAsync(request, cancellationToken);
        return File(image, "image/png");
    }
}
