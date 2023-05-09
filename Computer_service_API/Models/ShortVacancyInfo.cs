using System;
using System.Collections.Generic;

namespace Computer_service_API.Models
{
    public partial class ShortVacancyInfo
    {
        public int? VacId { get; set; }
        public string? DepName { get; set; } = null!;
        public double? VacSalary { get; set; }
        public string? VacComment { get; set; } = null!;
    }
}
