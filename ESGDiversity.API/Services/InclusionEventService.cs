using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IInclusionEventService
    {
        Task<PaginatedResponse<InclusionEventSummaryViewModel>> GetInclusionEventsAsync(
            int page,
            int pageSize,
            string? category);
        Task<InclusionEventResponse?> GetByIdAsync(int id);
        Task<InclusionEventResponse> CreateAsync(CreateInclusionEventRequest request);
        Task<InclusionEventResponse?> UpdateAsync(int id, UpdateInclusionEventRequest request);
        Task<bool> DeleteAsync(int id);
    }

    public class InclusionEventService : IInclusionEventService
    {
        private readonly ESGDiversityDbContext _context;

        public InclusionEventService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<InclusionEventSummaryViewModel>> GetInclusionEventsAsync(
            int page,
            int pageSize,
            string? category)
        {
            var query = _context.InclusionEvents.AsNoTracking();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            var totalCount = await query.CountAsync();
            var events = await query
                .OrderByDescending(e => e.EventDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var results = events.Select(e =>
            {
                var costPerParticipant = e.ParticipantsCount > 0
                    ? e.Budget / e.ParticipantsCount
                    : 0;

                var impactLevel = e.ParticipantsCount > 100
                    ? "High"
                    : e.ParticipantsCount > 50
                        ? "Medium"
                        : "Low";

                return new InclusionEventSummaryViewModel
                {
                    EventId = e.Id,
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Category = e.Category,
                    ParticipantsCount = e.ParticipantsCount,
                    Budget = e.Budget,
                    Department = e.Department,
                    Status = e.Status,
                    CostPerParticipant = Math.Round(costPerParticipant, 2),
                    ImpactLevel = impactLevel
                };
            }).ToList();

            return new PaginatedResponse<InclusionEventSummaryViewModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = results
            };
        }

        public async Task<InclusionEventResponse?> GetByIdAsync(int id)
        {
            var evt = await _context.InclusionEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return evt != null ? MapToResponse(evt) : null;
        }

        public async Task<InclusionEventResponse> CreateAsync(CreateInclusionEventRequest request)
        {
            var evt = new InclusionEvent
            {
                Title = request.Title,
                Description = request.Description,
                EventDate = request.EventDate,
                Category = request.Category,
                ParticipantsCount = request.ParticipantsCount,
                Budget = request.Budget,
                Department = request.Department,
                Status = "Scheduled",
                CreatedAt = DateTime.UtcNow
            };

            _context.InclusionEvents.Add(evt);
            await _context.SaveChangesAsync();

            return MapToResponse(evt);
        }

        public async Task<InclusionEventResponse?> UpdateAsync(int id, UpdateInclusionEventRequest request)
        {
            var evt = await _context.InclusionEvents.FindAsync(id);

            if (evt == null)
                return null;

            if (request.Title != null) evt.Title = request.Title;
            if (request.Description != null) evt.Description = request.Description;
            if (request.EventDate.HasValue) evt.EventDate = request.EventDate.Value;
            if (request.Category != null) evt.Category = request.Category;
            if (request.ParticipantsCount.HasValue) evt.ParticipantsCount = request.ParticipantsCount.Value;
            if (request.Budget.HasValue) evt.Budget = request.Budget.Value;
            if (request.Department != null) evt.Department = request.Department;
            if (request.Status != null) evt.Status = request.Status;

            await _context.SaveChangesAsync();

            return MapToResponse(evt);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var evt = await _context.InclusionEvents.FindAsync(id);

            if (evt == null)
                return false;

            _context.InclusionEvents.Remove(evt);
            await _context.SaveChangesAsync();

            return true;
        }

        private InclusionEventResponse MapToResponse(InclusionEvent evt)
        {
            return new InclusionEventResponse
            {
                Id = evt.Id,
                Title = evt.Title,
                Description = evt.Description,
                EventDate = evt.EventDate,
                Category = evt.Category,
                ParticipantsCount = evt.ParticipantsCount,
                Budget = evt.Budget,
                Department = evt.Department,
                Status = evt.Status,
                CreatedAt = evt.CreatedAt
            };
        }
    }
}
