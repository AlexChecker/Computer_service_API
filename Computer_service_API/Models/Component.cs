using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Component
    {


        public string? ArticleNum { get; set; } = null!;
        public int? Type { get; set; }
        public double? Price { get; set; }


    }
}
