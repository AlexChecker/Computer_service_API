using System;
using System.Collections.Generic;

namespace AdministrationPanel.Models
{
    public partial class Client
    {


        public string? Login { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public bool? Deleted { get; set; }


    }
}
