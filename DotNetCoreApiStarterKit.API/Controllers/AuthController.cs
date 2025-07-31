using DotNetCoreApiStarterKit.Domain.Interfaces.Services;
using DotNetCoreApiStarterKit.Shared.DTO_s.API.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreApiStarterKit.API.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }
    }
}
