using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class OrderHistory
    {
        public int? HistId { get; set; }
        public int? HistOrder { get; set; }
        public string? HistClient { get; set; } = null!;
        public DateTime? HistDate { get; set; }

    }
}
