namespace ESGDiversity.API.Models
{
    public class Employee
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
        public bool IsActive { get; set; } = true;
    }
}
