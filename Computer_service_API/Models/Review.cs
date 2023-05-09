using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Review
    {
        public int? RevId { get; set; }
        public string? RevText { get; set; } = null!;
        public string? RevAuthor { get; set; } = null!;

    }
}
