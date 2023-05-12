using System;
using System.Collections.Generic;

namespace AdministrationPanel.Models
{
    public partial class Order
    {


        public int? Id { get; set; }
        public string? Type { get; set; } = null!;
        public string? Client { get; set; } = null!;
        public string? Employee { get; set; } = null!;
        public double? Price { get; set; }
        public int? Status { get; set; }


    }
}
