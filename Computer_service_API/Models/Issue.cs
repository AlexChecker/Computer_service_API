using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Issue
    {
        public int? IssId { get; set; }
        public int? IssStatus { get; set; }
        public string? IssAuthor { get; set; } = null!;
        public string? IssAssistant { get; set; } = null!;

    }
}
