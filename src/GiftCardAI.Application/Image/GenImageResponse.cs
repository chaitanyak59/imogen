using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public class GenImageResponse
{
    public byte[] Content { get; set; }
    public int Seed { get; set; }

    public GenImageResponse(byte[] content, int _seed)
    {
        Content = content;
        Seed = _seed;
    }
}
