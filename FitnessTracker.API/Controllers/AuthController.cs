using FitnessTracker.Business.Abstractions;
using FitnessTracker.Business.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("users")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            await _authService.RegisterUserAsync(request.Login, request.Password, ct);
            return Created();
        }

        [HttpPost("token")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var token = await _authService.LoginUserAsync(request.Login, request.Password, ct);
            return Ok(new { Token = token });
        }
    }
}
