using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ESGDiversity.API.Data;
using ESGDiversity.API.Services;
using ESGDiversity.API.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ESG Diversity & Inclusion API",
        Version = "v1",
        Description = "API para gestão de diversidade e inclusão corporativa com foco em métricas ESG",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ESG Diversity Team",
            Email = "esg@company.com"
        }
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ESGDiversityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=ESGDiversityDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    )
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ESGDiversityAPI",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ESGDiversityAPI",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] ?? "YourSecretKeyHere12345678901234567890"
                )
            )
        };
    });

builder.Services.AddAuthorization();

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDiversityMetricsService, DiversityMetricsService>();
builder.Services.AddScoped<ISalaryEquityService, SalaryEquityService>();
builder.Services.AddScoped<IGoalProgressService, GoalProgressService>();
builder.Services.AddScoped<IInclusionEventService, InclusionEventService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDiversityGoalService, DiversityGoalService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Habilitar Swagger em todos os ambientes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESG Diversity & Inclusion API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Adicionar endpoint raiz para redirecionamento
app.MapGet("/", () => Results.Redirect("/index.html"))
    .ExcludeFromDescription();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ESGDiversityDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
