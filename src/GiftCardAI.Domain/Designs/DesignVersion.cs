using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Domain.Designs;

public class DesignVersion
{
    public Guid Id { get; } = Guid.NewGuid();

    public int VersionNumber { get; }

    public string Prompt { get; }

    public int Seed { get; }

    public string ImagePath { get; }

    public DateTime CreatedUtc { get; } = DateTime.UtcNow;

    public DesignVersion(int versionNumber, string prompt, int seed, string imagePath)
    {
        VersionNumber = versionNumber;
        Prompt = prompt;
        Seed = seed;
        ImagePath = imagePath;
    }
}
