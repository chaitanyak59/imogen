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
    private readonly IImageGenFactory _factory;
    private readonly GenImageValidator _validator;
    private readonly GenImagePromptBuilder _promptBuilder;
    private readonly IConfiguration _configuration;

    public ImageGenController(IImageGenFactory factory, GenImageValidator validator, GenImagePromptBuilder promptBuilder, IConfiguration configuration)
    {
        _factory = factory;
        _validator = validator;
        _promptBuilder = promptBuilder;
        _configuration = configuration;
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

        var _imageGenSvc = _factory.GetProvider(_configuration["AI:Provider"]! ?? "AWS");
        var prompt = _promptBuilder.Build(request.Occasion, request.Theme, request.Message);
        var image = await _imageGenSvc.GenerateAsync(prompt, cancellationToken);
        if (image?.Content is not { Length: > 0 })
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Image generation failed",
                detail: "The generated image is empty or unavailable."
            );
        }

        return File(image.Content, "image/png");
    }
}
