using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ESGDiversity.API.Data;
using ESGDiversity.API.Models;
using ESGDiversity.API.ViewModels;

namespace ESGDiversity.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse?> RegisterAsync(RegisterRequest request);
        Task<User?> GetUserByUsernameAsync(string username);
    }

    public class AuthService : IAuthService
    {
        private readonly ESGDiversityDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ESGDiversityDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

            if (user == null)
                return null;

            if (!VerifyPassword(request.Password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);
            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60");

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            };
        }

        public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
                return null;

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = request.Role,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60");

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            };
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyHere12345678901234567890")
            );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? "60");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "ESGDiversityAPI",
                audience: _configuration["Jwt:Audience"] ?? "ESGDiversityAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
