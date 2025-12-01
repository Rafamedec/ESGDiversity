using Microsoft.EntityFrameworkCore;
using ESGDiversity.API.Models;

namespace ESGDiversity.API.Data
{
    public class ESGDiversityDbContext : DbContext
    {
        public ESGDiversityDbContext(DbContextOptions<ESGDiversityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<DiversityGoal> DiversityGoals { get; set; }
        public DbSet<InclusionEvent> InclusionEvents { get; set; }
        public DbSet<SalaryEquityAnalysis> SalaryEquityAnalyses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<DiversityGoal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TargetPercentage).HasColumnType("decimal(5,2)");
                entity.Property(e => e.CurrentPercentage).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<InclusionEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Budget).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<SalaryEquityAnalysis>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AverageSalary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MedianSalary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PayGapPercentage).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var random = new Random(42);
            var genders = new[] { "Male", "Female", "Non-Binary", "Prefer not to say" };
            var ethnicities = new[] { "White", "Black", "Asian", "Hispanic", "Indigenous", "Mixed", "Other" };
            var departments = new[] { "IT", "HR", "Finance", "Marketing", "Sales", "Operations", "Engineering" };
            var positions = new[] { "Junior", "Mid-Level", "Senior", "Lead", "Manager", "Director" };
            var education = new[] { "High School", "Bachelor", "Master", "PhD" };

            var employees = new List<Employee>();
            for (int i = 1; i <= 500; i++)
            {
                employees.Add(new Employee
                {
                    Id = i,
                    Name = $"Employee {i}",
                    Email = $"employee{i}@company.com",
                    Gender = genders[random.Next(genders.Length)],
                    Ethnicity = ethnicities[random.Next(ethnicities.Length)],
                    Department = departments[random.Next(departments.Length)],
                    Position = positions[random.Next(positions.Length)],
                    Salary = random.Next(40000, 150000),
                    HireDate = DateTime.UtcNow.AddDays(-random.Next(1, 3650)),
                    IsDisabled = random.Next(100) < 5,
                    AgeGroup = random.Next(20, 65),
                    EducationLevel = education[random.Next(education.Length)],
                    IsActive = true
                });
            }

            modelBuilder.Entity<Employee>().HasData(employees);

            // Seed default users (password is "password123" hashed with SHA256)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@esgdiversity.com",
                    PasswordHash = "EF92B778BAFE771E89245B89ECBC08A44A4E166C06659911881F383D4473E94F", // password123
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    Username = "hr_manager",
                    Email = "hr@esgdiversity.com",
                    PasswordHash = "EF92B778BAFE771E89245B89ECBC08A44A4E166C06659911881F383D4473E94F", // password123
                    Role = "HR",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    Username = "user",
                    Email = "user@esgdiversity.com",
                    PasswordHash = "EF92B778BAFE771E89245B89ECBC08A44A4E166C06659911881F383D4473E94F", // password123
                    Role = "User",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );
        }
    }
}
