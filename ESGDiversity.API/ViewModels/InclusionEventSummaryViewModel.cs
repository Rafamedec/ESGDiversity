namespace ESGDiversity.API.ViewModels
{
    public class InclusionEventSummaryViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ParticipantsCount { get; set; }
        public decimal Budget { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CostPerParticipant { get; set; }
        public string ImpactLevel { get; set; } = string.Empty;
    }
}
