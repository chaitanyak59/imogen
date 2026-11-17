using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCardAI.Application.Image;
using Microsoft.Extensions.DependencyInjection;

namespace GiftCardAI.Infrastructure.Image;

public sealed class ImageGenFactory : IImageGenFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ImageGenFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IImageGenService GetProvider(string provider)
    {
        return provider.ToLowerInvariant() switch
        {
            "python" => _serviceProvider.GetRequiredKeyedService<IImageGenService>("Python"),
            "aws" => _serviceProvider.GetRequiredKeyedService<IImageGenService>("AWS"),
            _ => throw new ArgumentException("Unsupported AI provider.")
        };
    }
}
