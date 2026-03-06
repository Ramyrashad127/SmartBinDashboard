using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBin.Api.DTOs;
using SmartBin.Api.Services;
using System.Security.Claims;

namespace SmartBin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? throw new UnauthorizedAccessException());

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var (status, userId) = await _authService.RegisterAsync(model);

            if (status == "EMAIL_EXISTS")
            {
                return BadRequest(new { Message = "Email already in use." });
            }

            return Ok(new { Message = "User registered successfully.", UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var (token, userId) = await _authService.LoginAsync(model);

            if (token == "INVALID")
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            return Ok(new { Token = token, UserId = userId });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(GetUserId());
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
