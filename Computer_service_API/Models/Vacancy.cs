using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class Vacancy
    {
        public int? VacId { get; set; }
        public int? VacDepartment { get; set; }
        public double? VacSalary { get; set; }
        public string? VacComment { get; set; } = null!;

    }
}
