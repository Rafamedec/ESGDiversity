namespace ESGDiversity.API.Models
{
    public class SalaryEquityAnalysis
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public decimal AverageSalary { get; set; }
        public decimal MedianSalary { get; set; }
        public int EmployeeCount { get; set; }
        public decimal PayGapPercentage { get; set; }
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;
    }
}
