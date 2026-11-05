using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public interface IImageGenService
{
    Task<GenImageResponse> GenerateAsync(string prompt, CancellationToken cancellationToken = default);
}
