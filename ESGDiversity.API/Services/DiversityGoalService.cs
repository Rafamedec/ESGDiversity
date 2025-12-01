using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IDiversityGoalService
    {
        Task<PaginatedResponse<DiversityGoalResponse>> GetAllAsync(int page, int pageSize);
        Task<DiversityGoalResponse?> GetByIdAsync(int id);
        Task<DiversityGoalResponse> CreateAsync(CreateDiversityGoalRequest request);
        Task<DiversityGoalResponse?> UpdateAsync(int id, UpdateDiversityGoalRequest request);
        Task<bool> DeleteAsync(int id);
    }

    public class DiversityGoalService : IDiversityGoalService
    {
        private readonly ESGDiversityDbContext _context;

        public DiversityGoalService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<DiversityGoalResponse>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.DiversityGoals.AsNoTracking();

            var totalCount = await query.CountAsync();
            var goals = await query
                .OrderByDescending(g => g.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = goals.Select(g => MapToResponse(g)).ToList();

            return new PaginatedResponse<DiversityGoalResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = response
            };
        }

        public async Task<DiversityGoalResponse?> GetByIdAsync(int id)
        {
            var goal = await _context.DiversityGoals
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return goal != null ? MapToResponse(goal) : null;
        }

        public async Task<DiversityGoalResponse> CreateAsync(CreateDiversityGoalRequest request)
        {
            var goal = new DiversityGoal
            {
                Department = request.Department,
                MetricType = request.MetricType,
                TargetPercentage = request.TargetPercentage,
                CurrentPercentage = request.CurrentPercentage,
                TargetDate = request.TargetDate,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                Status = "Active"
            };

            _context.DiversityGoals.Add(goal);
            await _context.SaveChangesAsync();

            return MapToResponse(goal);
        }

        public async Task<DiversityGoalResponse?> UpdateAsync(int id, UpdateDiversityGoalRequest request)
        {
            var goal = await _context.DiversityGoals.FindAsync(id);

            if (goal == null)
                return null;

            if (request.Department != null) goal.Department = request.Department;
            if (request.MetricType != null) goal.MetricType = request.MetricType;
            if (request.TargetPercentage.HasValue) goal.TargetPercentage = request.TargetPercentage.Value;
            if (request.CurrentPercentage.HasValue) goal.CurrentPercentage = request.CurrentPercentage.Value;
            if (request.TargetDate.HasValue) goal.TargetDate = request.TargetDate.Value;
            if (request.Status != null) goal.Status = request.Status;
            if (request.Notes != null) goal.Notes = request.Notes;

            await _context.SaveChangesAsync();

            return MapToResponse(goal);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var goal = await _context.DiversityGoals.FindAsync(id);

            if (goal == null)
                return false;

            _context.DiversityGoals.Remove(goal);
            await _context.SaveChangesAsync();

            return true;
        }

        private DiversityGoalResponse MapToResponse(DiversityGoal goal)
        {
            return new DiversityGoalResponse
            {
                Id = goal.Id,
                Department = goal.Department,
                MetricType = goal.MetricType,
                TargetPercentage = goal.TargetPercentage,
                CurrentPercentage = goal.CurrentPercentage,
                TargetDate = goal.TargetDate,
                CreatedAt = goal.CreatedAt,
                Status = goal.Status,
                Notes = goal.Notes
            };
        }
    }
}
