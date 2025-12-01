using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IDiversityMetricsService
    {
        Task<PaginatedResponse<DiversityMetricsViewModel>> GetDiversityMetricsAsync(
            int page,
            int pageSize,
            string? department);
    }

    public class DiversityMetricsService : IDiversityMetricsService
    {
        private readonly ESGDiversityDbContext _context;

        public DiversityMetricsService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<DiversityMetricsViewModel>> GetDiversityMetricsAsync(
            int page,
            int pageSize,
            string? department)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }

            var departments = await query
                .Select(e => e.Department)
                .Distinct()
                .ToListAsync();

            var totalCount = departments.Count;
            var pagedDepartments = departments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var metrics = new List<DiversityMetricsViewModel>();

            foreach (var dept in pagedDepartments)
            {
                var deptEmployees = await query
                    .Where(e => e.Department == dept)
                    .ToListAsync();

                var total = deptEmployees.Count;
                var women = deptEmployees.Count(e => e.Gender == "Female");
                var minorities = deptEmployees.Count(e => e.Ethnicity != "White");
                var disabled = deptEmployees.Count(e => e.IsDisabled);

                var ethnicityDist = deptEmployees
                    .GroupBy(e => e.Ethnicity)
                    .ToDictionary(g => g.Key, g => g.Count());

                var ageDist = deptEmployees
                    .GroupBy(e => e.AgeGroup / 10 * 10)
                    .ToDictionary(g => $"{g.Key}-{g.Key + 9}", g => g.Count());

                var diversityScore = ((women / (double)total * 100) +
                                    (minorities / (double)total * 100) +
                                    (disabled / (double)total * 100)) / 3;

                metrics.Add(new DiversityMetricsViewModel
                {
                    Department = dept,
                    WomenPercentage = Math.Round((decimal)(women / (double)total * 100), 2),
                    MinorityPercentage = Math.Round((decimal)(minorities / (double)total * 100), 2),
                    DisabledPercentage = Math.Round((decimal)(disabled / (double)total * 100), 2),
                    TotalEmployees = total,
                    DiversityScore = Math.Round((decimal)diversityScore, 2),
                    EthnicityDistribution = ethnicityDist,
                    AgeGroupDistribution = ageDist
                });
            }

            return new PaginatedResponse<DiversityMetricsViewModel>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = metrics
            };
        }
    }
}
