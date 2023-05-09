using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Acquisition
    {
        public int? AcqId { get; set; }
        public string? Component { get; set; } = null!;
        public double? Price { get; set; }
        public int? Amount { get; set; }

    }
}
