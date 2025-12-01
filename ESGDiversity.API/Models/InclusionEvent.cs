namespace ESGDiversity.API.Models
{
    public class InclusionEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ParticipantsCount { get; set; }
        public decimal Budget { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
