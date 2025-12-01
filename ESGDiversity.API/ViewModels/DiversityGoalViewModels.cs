namespace ESGDiversity.API.ViewModels
{
    public class CreateDiversityGoalRequest
    {
        public string Department { get; set; } = string.Empty;
        public string MetricType { get; set; } = string.Empty;
        public decimal TargetPercentage { get; set; }
        public decimal CurrentPercentage { get; set; }
        public DateTime TargetDate { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateDiversityGoalRequest
    {
        public string? Department { get; set; }
        public string? MetricType { get; set; }
        public decimal? TargetPercentage { get; set; }
        public decimal? CurrentPercentage { get; set; }
        public DateTime? TargetDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class DiversityGoalResponse
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public string MetricType { get; set; } = string.Empty;
        public decimal TargetPercentage { get; set; }
        public decimal CurrentPercentage { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
