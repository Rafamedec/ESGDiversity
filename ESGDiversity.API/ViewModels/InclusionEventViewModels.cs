namespace ESGDiversity.API.ViewModels
{
    public class CreateInclusionEventRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ParticipantsCount { get; set; }
        public decimal Budget { get; set; }
        public string Department { get; set; } = string.Empty;
    }

    public class UpdateInclusionEventRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Category { get; set; }
        public int? ParticipantsCount { get; set; }
        public decimal? Budget { get; set; }
        public string? Department { get; set; }
        public string? Status { get; set; }
    }

    public class InclusionEventResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int ParticipantsCount { get; set; }
        public decimal Budget { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
