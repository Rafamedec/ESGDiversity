namespace ESGDiversity.API.ViewModels
{
    public class SalaryEquityViewModel
    {
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal MaleAverageSalary { get; set; }
        public decimal FemaleAverageSalary { get; set; }
        public decimal PayGapPercentage { get; set; }
        public decimal PayGapAmount { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
        public string EquityStatus { get; set; } = string.Empty;
    }
}
