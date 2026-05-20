using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ApiProyectoWeb.Models.DTOs;

namespace ApiProyectoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _service;

        public AttendanceController(IAttendanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("plan/{planId}")]
        public async Task<IActionResult> GetByPlan(Guid planId)
        {
            var result = await _service.GetByPlan(planId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null) return NotFound(new { message = "Error 404: La asistencia no fue encontrada." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetAttendance([FromBody] SetAttendanceDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _service.SetAttendance(dto, userId);
                if (result == null) return BadRequest(new { message = "Error 400: No se puede confirmar asistencia." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo registrar la asistencia. {ex.Message}" });
            }
        }

        [HttpPatch("{planId}/checkin")]
        public async Task<IActionResult> CheckIn(Guid planId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _service.CheckIn(planId, userId);
                if (result == null) return BadRequest(new { message = "Error 400: Check-in fallido. Verifica si ya tienes asistencia o el plan no está activo." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: Error al hacer check-in. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Attendance editAttendance)
        {
            try
            {
                var result = await _service.Edit(id, editAttendance);
                if (!result) return NotFound(new { message = "Error 404: La asistencia no existe." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            try
            {
                var result = await _service.ChangeStatus(id);
                if (result == -1) return NotFound(new { message = "Error 404: La asistencia no existe." });
                return Ok(new { isActive = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
