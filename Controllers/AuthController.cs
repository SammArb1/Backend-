using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyectoWeb.Controllers
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
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == "Unauthorized")
                return Unauthorized();
            return Ok(new { token = result });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result == "User created successfully!")
                return Ok(new { message = result });
            return BadRequest(new { message = result });
        }
    }
}
