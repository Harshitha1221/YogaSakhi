using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YogaSakhi.Domain.Entities;
using YogaSakhi.Domain.Interfaces;
using BCrypt.Net;

namespace YogaSakhi.Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpirationMinutes;

        public AuthService(IUnitOfWork unitOfWork, AuthSettings settings)
        {
            _unitOfWork = unitOfWork;
            _jwtSecret = settings.JwtSecret;
            _jwtIssuer = settings.JwtIssuer;
            _jwtAudience = settings.JwtAudience;
            _jwtExpirationMinutes = settings.JwtExpirationMinutes;
        }

        public async Task<(bool Success, string Token, string Message)> RegisterAsync(
            string email, 
            string password, 
            string fullName, 
            int age)
        {
            try
            {
                // Check if user exists
                var existingUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                    return (false, null, "User already exists");

                // Validate password
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    return (false, null, "Password must be at least 6 characters");

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                // Create user
                var user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    Email = email,
                    FullName = fullName,
                    Age = age,
                    PasswordHash = passwordHash,
                    Role = UserRole.User,
                    MembershipDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Create user profile
                var profile = new UserProfile
                {
                    UserId = user.Id,
                    PregnancyStatus = "NotPregnant",
                    FitnessLevel = "Beginner",
                    PreferredLanguage = "English",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                await _unitOfWork.UserProfiles.AddAsync(profile);
                await _unitOfWork.SaveChangesAsync();

                // Generate JWT token
                var token = await GenerateJwtTokenAsync(user);

                return (true, token, "Registration successful");
            }
            catch (Exception ex)
            {
                return (false, null, $"Registration failed: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(
            string email, 
            string password)
        {
            try
            {
                var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    return (false, null, "Invalid email or password");

                if (!user.IsActive)
                    return (false, null, "User account is inactive");

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    return (false, null, "Invalid email or password");

                // Generate JWT token
                var token = await GenerateJwtTokenAsync(user);

                return (true, token, "Login successful");
            }
            catch (Exception ex)
            {
                return (false, null, $"Login failed: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> RefreshTokenAsync(string token)
        {
            try
            {
                // Validate and decode token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = false // Allow expired tokens for refresh
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.Find(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return (false, "User not found");

                var newToken = await GenerateJwtTokenAsync(user);
                return (true, newToken);
            }
            catch (Exception ex)
            {
                return (false, $"Token refresh failed: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> VerifyEmailAsync(string email, string code)
        {
            // TODO: Implement email verification
            return (true, "Email verified");
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("userId", user.UserId),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class AuthSettings
    {
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpirationMinutes { get; set; }
    }
}
