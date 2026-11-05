using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Domain.Designs;

public sealed class DesignSession
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime CreatedUtc { get; } = DateTime.UtcNow;

    public List<DesignVersion> Versions = [];
}