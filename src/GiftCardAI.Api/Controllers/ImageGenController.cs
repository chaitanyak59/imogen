using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardAI.Application.Image;
using GiftCardAI.Application.Image.Validator;
using Microsoft.AspNetCore.Mvc;

namespace GiftCardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageGenController : ControllerBase
{
    private readonly IImageGenService _imageGenSvc;
    private readonly GenImageValidator _validator;
    private readonly GenImagePromptBuilder _promptBuilder;

    public ImageGenController(IImageGenService imageGenSvc, GenImageValidator validator, GenImagePromptBuilder promptBuilder)
    {
        _imageGenSvc = imageGenSvc;
        _validator = validator;
        _promptBuilder = promptBuilder;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(GenImageRequest request, CancellationToken cancellationToken)
    {
        if (!_validator.IsValid(request.Occasion, request.Theme))
        {
            return BadRequest(new
            {
                message = "Unsupported gift card occasion or theme."
            });
        }

        var prompt = _promptBuilder.Build(request.Occasion, request.Theme, request.Message);
        var image = await _imageGenSvc.GenerateAsync(prompt, cancellationToken);
        return File(image.Content, "image/png");
    }
}
