using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IGoalProgressService
    {
        Task<PaginatedResponse<GoalProgressViewModel>> GetGoalProgressAsync(int page, int pageSize);
    }

    public class GoalProgressService : IGoalProgressService
    {
        private readonly ESGDiversityDbContext _context;

        public GoalProgressService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<GoalProgressViewModel>> GetGoalProgressAsync(
            int page,
            int pageSize)
        {
            var goals = await _context.DiversityGoals
                .AsNoTracking()
                .Where(g => g.Status == "Active")
                .ToListAsync();

            var totalCount = goals.Count;
            var pagedGoals = goals
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var results = new List<GoalProgressViewModel>();

            foreach (var goal in pagedGoals)
            {
                var daysRemaining = (goal.TargetDate - DateTime.UtcNow).Days;
                var progress = goal.TargetPercentage > 0
                    ? (goal.CurrentPercentage / goal.TargetPercentage) * 100
                    : 0;

                var isOnTrack = progress >= 50 && daysRemaining > 30 ||
                               progress >= 75 && daysRemaining > 0;

                var status = progress >= 100
                    ? "Completed"
                    : isOnTrack
                        ? "On Track"
                        : "At Risk";

                var actions = !isOnTrack
                    ? "Increase hiring efforts, review recruitment strategies"
                    : "Continue current strategy";

                results.Add(new GoalProgressViewModel
                {
                    GoalId = goal.Id,
                    Department = goal.Department,
                    MetricType = goal.MetricType,
                    TargetPercentage = goal.TargetPercentage,
                    CurrentPercentage = goal.CurrentPercentage,
                    ProgressPercentage = Math.Round(progress, 2),
                    DaysRemaining = daysRemaining,
                    Status = status,
                    IsOnTrack = isOnTrack,
                    RecommendedActions = actions
                });
            }

            return new PaginatedResponse<GoalProgressViewModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = results
            };
        }
    }
}
