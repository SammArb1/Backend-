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
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _service;

        public PlanController(IPlanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("parche/{parcheId}")]
        public async Task<IActionResult> GetByParche(Guid parcheId)
        {
            var result = await _service.GetByParche(parcheId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null) return NotFound(new { message = "Error 404: El plan no fue encontrado." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlanDto newPlan)
        {
            if (string.IsNullOrWhiteSpace(newPlan.Title))
                return BadRequest(new { message = "Error 400: El título del plan no puede estar vacío." });
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _service.CreatePlanWithOptions(newPlan, userId);
                return CreatedAtAction(nameof(GetById), new { id = result.id_plan }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo crear el plan. {ex.Message}" });
            }
        }

        [HttpPatch("{id}/transition")]
        public async Task<IActionResult> TransitionState(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _service.TransitionState(id, userId);
                if (result == null) return NotFound(new { message = "Error 404: No se encontró el plan o no tienes permisos." });
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo transicionar el plan. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Plan editPlan)
        {
            try
            {
                var result = await _service.Edit(id, editPlan);
                if (!result) return NotFound(new { message = "Error 404: El plan no existe." });
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
                if (result == -1) return NotFound(new { message = "Error 404: El plan no existe." });
                return Ok(new { isActive = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
