using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public class GenImageResponse
{
    public byte[] Content { get; set; }
    public long? Seed { get; set; }

    public GenImageResponse(byte[] content, long _seed = default)
    {
        Content = content;
        Seed = _seed;
    }
}
