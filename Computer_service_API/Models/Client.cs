using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Client
    {


        public string? Login { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public bool? Deleted { get; set; }
        public string? Token { get; set; } = null!;


    }
}
