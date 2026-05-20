using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiProyectoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParcheController : ControllerBase
    {
        private readonly IParcheService _parcheService;

        public ParcheController(IParcheService parcheService)
        {
            _parcheService = parcheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _parcheService.GetAll();
            return Ok(result);
        }

        [HttpGet("my-parches")]
        public async Task<IActionResult> GetMyParches()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _parcheService.GetMyParches(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _parcheService.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Parche newParche)
        {
            if (string.IsNullOrWhiteSpace(newParche.name))
                return BadRequest(new { message = "Error 400: El nombre del parche no puede estar vacío." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _parcheService.Create(newParche, userId);
                return CreatedAtAction(nameof(GetById), new { id = result.id_parche }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo crear el parche. {ex.Message}" });
            }
        }

        [HttpPost("join/{code}")]
        public async Task<IActionResult> Join(string code)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _parcheService.Join(code, userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo unir al parche. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Parche editParche)
        {
            var result = await _parcheService.Edit(id, editParche);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            var result = await _parcheService.ChangeStatus(id);
            if (result == -1) return NotFound();
            return Ok(new { isActive = result });
        }

        [HttpPost("{id}/leave")]
        public async Task<IActionResult> Leave(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            var result = await _parcheService.Leave(id, userId);
            if (!result) return BadRequest(new { message = "Error 400: No se pudo salir del parche o no eres miembro." });
            return Ok(new { message = "Has salido del parche exitosamente." });
        }

        [HttpPatch("{parcheId}/member/{targetUserId}/role")]
        public async Task<IActionResult> SetMemberRole(Guid parcheId, string targetUserId, [FromBody] ApiProyectoWeb.Models.DTOs.SetRoleDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado." });
            
            try
            {
                var result = await _parcheService.SetMemberRole(parcheId, targetUserId, dto.Role, userId);
                return Ok(new { success = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpDelete("{parcheId}/member/{targetUserId}")]
        public async Task<IActionResult> RemoveMember(Guid parcheId, string targetUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado." });
            
            try
            {
                var result = await _parcheService.RemoveMember(parcheId, targetUserId, userId);
                return Ok(new { success = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
