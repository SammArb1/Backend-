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
            try
            {
                var result = await _authService.LoginAsync(model);
                if (result == "Unauthorized")
                    return Unauthorized(new { message = "Error 401: El correo o la contraseña son incorrectos." });
                return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo iniciar sesión. {ex.Message}" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (result == "User created successfully!")
                    return Ok(new { message = result });
                return BadRequest(new { message = $"Error 400: {result}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo registrar el usuario. {ex.Message}" });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _authService.GetUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudieron obtener los usuarios. {ex.Message}" });
            }
        }
    }
}
