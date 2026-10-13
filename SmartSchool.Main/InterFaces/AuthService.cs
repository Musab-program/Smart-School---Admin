using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartSchool.Core.Models;
using SmartSchool.Main.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartSchool.Main.InterFaces
{
    /// <summary>
    /// Handles all JWT authentication operations:
    /// login, token generation, token refresh, and logout.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        /// <summary>
        /// Validates credentials, generates JWT + refresh token, persists refresh token to DB.
        /// </summary>
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // 1. Find user by email
            var user = await _userManager.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsActive)
                return null;

            // 2. Verify password using Identity's built-in hasher
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return null;

            // 3. Generate tokens
            var (accessToken, expiresAt) = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            // 4. Save refresh token in DB
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                _config.GetValue<int>("Jwt:RefreshTokenDays"));
            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                UserName = user.Name,
                Email = user.Email!,
                Role = user.Role?.Name ?? string.Empty
            };
        }

        /// <summary>
        /// Issues a new access + refresh token pair if the refresh token is valid and not expired.
        /// </summary>
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == refreshToken &&
                    u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return null;

            var (accessToken, expiresAt) = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
                _config.GetValue<int>("Jwt:RefreshTokenDays"));
            await _userManager.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt,
                UserName = user.Name,
                Email = user.Email!,
                Role = user.Role?.Name ?? string.Empty
            };
        }

        /// <summary>
        /// Clears the refresh token, effectively logging the user out from all sessions.
        /// </summary>
        public async Task<bool> LogoutAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

        // ─────────────────────────────────────────────────────────────────
        // Private Helpers
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Builds a signed JWT with claims: NameIdentifier, Email, Name, Role.
        /// </summary>
        private (string token, DateTime expiresAt) GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty),
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("Jwt:AccessTokenMinutes"));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        /// <summary>
        /// Generates a cryptographically secure random refresh token (Base64 URL-safe string).
        /// </summary>
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Interface
    // ─────────────────────────────────────────────────────────────────────────
    public interface IAuthService
    {
        /// <summary>Login and get JWT + refresh token. Returns null if credentials are invalid.</summary>
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

        /// <summary>Exchange a valid refresh token for a new token pair.</summary>
        Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);

        /// <summary>Logout by clearing the stored refresh token.</summary>
        Task<bool> LogoutAsync(int userId);
    }
}
