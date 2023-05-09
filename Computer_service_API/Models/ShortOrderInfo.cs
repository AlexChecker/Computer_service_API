using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class ShortOrderInfo
    {
        public int? OrderId { get; set; }
        public string? OrderType { get; set; } = null!;
        public double? OrderPrice { get; set; }
        public string? OrderStatus { get; set; } = null!;
        public string? EmployeeName { get; set; } = null!;
        public string? ClientLogin { get; set; } = null!;
    }
}
