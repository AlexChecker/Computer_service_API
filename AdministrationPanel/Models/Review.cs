using System;
using System.Collections.Generic;

namespace AdministrationPanel.Models
{
    public partial class Review
    {
        public int? RevId { get; set; }
        public string? RevText { get; set; } = null!;
        public string? RevAuthor { get; set; } = null!;

    }
}
