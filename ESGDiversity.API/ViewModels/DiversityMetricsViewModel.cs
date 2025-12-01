namespace ESGDiversity.API.ViewModels
{
    public class DiversityMetricsViewModel
    {
        public string Department { get; set; } = string.Empty;
        public decimal WomenPercentage { get; set; }
        public decimal MinorityPercentage { get; set; }
        public decimal DisabledPercentage { get; set; }
        public int TotalEmployees { get; set; }
        public decimal DiversityScore { get; set; }
        public Dictionary<string, int> EthnicityDistribution { get; set; } = new();
        public Dictionary<string, int> AgeGroupDistribution { get; set; } = new();
    }
}
