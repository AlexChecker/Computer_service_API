namespace Computer_service_API.Models
{
    public class EmployeeModel
    {
        public string? first_name { get; set; } = null!;
        public string? last_name { get; set; } = null!;
        public string? login { get; set; } = null!;
        public string? password { get; set; } = null!;
        public int? department { get; set; }
    }
}
