using System;
using System.Collections.Generic;

namespace AdministrationPanel.Models
{
    public partial class ComponentUsage
    {
        public int? UsageId { get; set; }
        public string? Component { get; set; } = null!;
        public int? Order { get; set; }

    }
}
