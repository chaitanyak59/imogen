using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GiftCardAI.Application.Image;

public sealed record GenImageRequest(
    string Occasion,
    string Theme,
    string Message
);