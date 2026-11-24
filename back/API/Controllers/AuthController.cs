using API.Extensions;
using BLL.DTO.Auth;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO dto)
        {
            var response = await _authService.LoginAsync(dto);
            return this.GetResult(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO dto)
        {
            var response = await _authService.RegisterAsync(dto);
            return this.GetResult(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromQuery] string userId,
            [FromQuery] string token)
        {
            var response = await _authService.ConfirmEmailAsync(userId, token);
            return this.GetResult(response);
        }
    }
}
