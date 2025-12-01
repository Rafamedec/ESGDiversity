namespace ESGDiversity.API.Models
{
    public class DiversityGoal
    {
        public int Id { get; set; }
        public string Department { get; set; } = string.Empty;
        public string MetricType { get; set; } = string.Empty;
        public decimal TargetPercentage { get; set; }
        public decimal CurrentPercentage { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active";
        public string? Notes { get; set; }
    }
}
