using System;
using System.Collections.Generic;

namespace AdministrationPanel.Models
{
    public partial class OrderHistory
    {
        public int? HistId { get; set; }
        public int? HistOrder { get; set; }
        public string? HistClient { get; set; } = null!;
        public DateTime? HistDate { get; set; }

    }
}
