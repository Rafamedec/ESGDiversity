using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IEmployeeService
    {
        Task<PaginatedResponse<EmployeeResponse>> GetAllAsync(int page, int pageSize, string? department = null);
        Task<EmployeeResponse?> GetByIdAsync(int id);
        Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
        Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request);
        Task<bool> DeleteAsync(int id);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly ESGDiversityDbContext _context;

        public EmployeeService(ESGDiversityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<EmployeeResponse>> GetAllAsync(
            int page,
            int pageSize,
            string? department = null)
        {
            var query = _context.Employees.AsNoTracking().Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }

            var totalCount = await query.CountAsync();
            var employees = await query
                .OrderBy(e => e.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = employees.Select(e => MapToResponse(e)).ToList();

            return new PaginatedResponse<EmployeeResponse>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = response
            };
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            return employee != null ? MapToResponse(employee) : null;
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
        {
            var employee = new Employee
            {
                Name = request.Name,
                Email = request.Email,
                Gender = request.Gender,
                Ethnicity = request.Ethnicity,
                Department = request.Department,
                Position = request.Position,
                Salary = request.Salary,
                HireDate = request.HireDate,
                IsDisabled = request.IsDisabled,
                AgeGroup = request.AgeGroup,
                EducationLevel = request.EducationLevel,
                IsActive = true
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return MapToResponse(employee);
        }

        public async Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || !employee.IsActive)
                return null;

            if (request.Name != null) employee.Name = request.Name;
            if (request.Email != null) employee.Email = request.Email;
            if (request.Gender != null) employee.Gender = request.Gender;
            if (request.Ethnicity != null) employee.Ethnicity = request.Ethnicity;
            if (request.Department != null) employee.Department = request.Department;
            if (request.Position != null) employee.Position = request.Position;
            if (request.Salary.HasValue) employee.Salary = request.Salary.Value;
            if (request.HireDate.HasValue) employee.HireDate = request.HireDate.Value;
            if (request.IsDisabled.HasValue) employee.IsDisabled = request.IsDisabled.Value;
            if (request.AgeGroup.HasValue) employee.AgeGroup = request.AgeGroup.Value;
            if (request.EducationLevel != null) employee.EducationLevel = request.EducationLevel;
            if (request.IsActive.HasValue) employee.IsActive = request.IsActive.Value;

            await _context.SaveChangesAsync();

            return MapToResponse(employee);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        private EmployeeResponse MapToResponse(Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Gender = employee.Gender,
                Ethnicity = employee.Ethnicity,
                Department = employee.Department,
                Position = employee.Position,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                IsDisabled = employee.IsDisabled,
                AgeGroup = employee.AgeGroup,
                EducationLevel = employee.EducationLevel,
                IsActive = employee.IsActive
            };
        }
    }
}
