using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public sealed record GenImageRequest
{
    [Required]
    [StringLength(1000, MinimumLength = 3)]
    public required string Prompt { get; init; }
}