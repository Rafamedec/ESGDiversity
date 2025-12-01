using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface ISalaryEquityService
    {
        Task<PaginatedResponse<SalaryEquityViewModel>> GetSalaryEquityAnalysisAsync(
            int page,
            int pageSize,
            string? department);
    }

    public class SalaryEquityService : ISalaryEquityService
    {
        private readonly ESGDiversityDbContext _context;

        public SalaryEquityService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<SalaryEquityViewModel>> GetSalaryEquityAnalysisAsync(
            int page,
            int pageSize,
            string? department)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Where(e => e.IsActive && (e.Gender == "Male" || e.Gender == "Female"));

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }

            var groupedData = await query
                .GroupBy(e => new { e.Department, e.Position })
                .Select(g => new { g.Key.Department, g.Key.Position })
                .ToListAsync();

            var totalCount = groupedData.Count;
            var pagedData = groupedData
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var results = new List<SalaryEquityViewModel>();

            foreach (var group in pagedData)
            {
                var males = await query
                    .Where(e => e.Department == group.Department &&
                               e.Position == group.Position &&
                               e.Gender == "Male")
                    .ToListAsync();

                var females = await query
                    .Where(e => e.Department == group.Department &&
                               e.Position == group.Position &&
                               e.Gender == "Female")
                    .ToListAsync();

                if (males.Any() && females.Any())
                {
                    var maleAvg = males.Average(e => e.Salary);
                    var femaleAvg = females.Average(e => e.Salary);
                    var payGap = ((maleAvg - femaleAvg) / maleAvg) * 100;

                    var status = payGap <= 5
                        ? "Equitable"
                        : payGap <= 15
                            ? "Needs Attention"
                            : "Critical";

                    results.Add(new SalaryEquityViewModel
                    {
                        Department = group.Department,
                        Position = group.Position,
                        MaleAverageSalary = Math.Round(maleAvg, 2),
                        FemaleAverageSalary = Math.Round(femaleAvg, 2),
                        PayGapPercentage = Math.Round(payGap, 2),
                        PayGapAmount = Math.Round(maleAvg - femaleAvg, 2),
                        MaleCount = males.Count,
                        FemaleCount = females.Count,
                        EquityStatus = status
                    });
                }
            }

            return new PaginatedResponse<SalaryEquityViewModel>
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
