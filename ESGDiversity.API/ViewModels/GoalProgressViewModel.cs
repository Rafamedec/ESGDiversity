namespace ESGDiversity.API.ViewModels
{
    public class GoalProgressViewModel
    {
        public int GoalId { get; set; }
        public string Department { get; set; } = string.Empty;
        public string MetricType { get; set; } = string.Empty;
        public decimal TargetPercentage { get; set; }
        public decimal CurrentPercentage { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int DaysRemaining { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsOnTrack { get; set; }
        public string RecommendedActions { get; set; } = string.Empty;
    }
}
