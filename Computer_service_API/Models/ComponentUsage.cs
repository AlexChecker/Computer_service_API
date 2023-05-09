using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class ComponentUsage
    {
        public int? UsageId { get; set; }
        public string? Component { get; set; } = null!;
        public int? Order { get; set; }

    }
}
