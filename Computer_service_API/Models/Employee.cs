using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Employee
    {


        public string? ServiceId { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? SecondName { get; set; } = null!;
        public string? Login { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public int? Department { get; set; }
        public bool? Deleted { get; set; }
        public string? Token { get; set; } = null!;

    }
}
