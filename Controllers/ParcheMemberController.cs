using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyectoWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParcheMemberController : ControllerBase
    {
        private readonly IParcheMemberService _service;

        public ParcheMemberController(IParcheMemberService service)
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
                if (result == null) return NotFound(new { message = "Error 404: Miembro de parche no encontrado." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParcheMember newMember)
        {
            try
            {
                var result = await _service.Create(newMember);
                return CreatedAtAction(nameof(GetById), new { id = result.id_parche_member }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo crear el miembro del parche. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ParcheMember editMember)
        {
            try
            {
                var result = await _service.Edit(id, editMember);
                if (!result) return NotFound(new { message = "Error 404: Miembro no existe." });
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
                if (result == -1) return NotFound(new { message = "Error 404: Miembro no existe." });
                return Ok(new { isActive = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
