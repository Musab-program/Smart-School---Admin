using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;
using System.Security.Claims;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login with email + password. Returns a JWT access token and a refresh token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { Message = "البريد أو كلمة المرور غير صحيحة" });

            return Ok(result);
        }

        /// <summary>
        /// Exchange a valid refresh token for a new access + refresh token pair.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest(new { Message = "الـ Refresh Token مطلوب" });

            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (result == null)
                return Unauthorized(new { Message = "الـ Refresh Token منتهي أو غير صحيح" });

            return Ok(result);
        }

        /// <summary>
        /// Logout: clears the stored refresh token so it can no longer be used.
        /// Requires a valid JWT in the Authorization header.
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Extract user ID from the JWT claims
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _authService.LogoutAsync(userId);
            return Ok(new { Message = "تم تسجيل الخروج بنجاح" });
        }
    }
}
