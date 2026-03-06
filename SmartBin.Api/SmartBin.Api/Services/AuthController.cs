using Microsoft.AspNetCore.Mvc;
using SmartBin.Api.Models;   
using SmartBin.Api.Services; 

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var result = await _authService.RegisterAsync(model);

            if (result == "0")
            {
                return BadRequest(new { Message = result });
            }

            return Ok(new { Message = result });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _authService.LoginAsync(model);

            if (result == "0")
            {
                // Returns 401 Unauthorized
                return Unauthorized(new { Message = result });
            }

            // Returns 200 OK
            return Ok(new { Message = result });
        }
    }
}
