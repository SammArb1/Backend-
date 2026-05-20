using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyectoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlanOptionController : ControllerBase
    {
        private readonly IPlanOptionService _service;

        public PlanOptionController(IPlanOptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null) return NotFound(new { message = "Error 404: La opción de plan no fue encontrada." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlanOption newOption)
        {
            try
            {
                var result = await _service.Create(newOption);
                return CreatedAtAction(nameof(GetById), new { id = result.id_plan_option }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo crear la opción de plan. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PlanOption editOption)
        {
            try
            {
                var result = await _service.Edit(id, editOption);
                if (!result) return NotFound(new { message = "Error 404: La opción de plan no existe." });
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
                if (result == -1) return NotFound(new { message = "Error 404: La opción de plan no existe." });
                return Ok(new { isActive = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
