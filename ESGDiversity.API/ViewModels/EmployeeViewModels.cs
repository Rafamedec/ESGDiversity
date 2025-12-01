namespace ESGDiversity.API.ViewModels
{
    public class CreateEmployeeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Ethnicity { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsDisabled { get; set; }
        public int AgeGroup { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
    }

    public class UpdateEmployeeRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Ethnicity { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? HireDate { get; set; }
        public bool? IsDisabled { get; set; }
        public int? AgeGroup { get; set; }
        public string? EducationLevel { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Ethnicity { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsDisabled { get; set; }
        public int AgeGroup { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
