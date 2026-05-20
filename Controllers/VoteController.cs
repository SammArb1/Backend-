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
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _service;

        public VoteController(IVoteService service)
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
                if (result == null) return NotFound(new { message = "Error 404: El voto no fue encontrado." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CastVote([FromBody] CastVoteDto voteDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { message = "Error 401: Tu sesión ha expirado, por favor vuelve a iniciar sesión." });
            
            try
            {
                var result = await _service.CastVote(voteDto, userId);
                if (result == null) return BadRequest(new { message = "Error 400: No se puede votar en este plan en este momento." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: No se pudo emitir el voto. {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Vote editVote)
        {
            try
            {
                var result = await _service.Edit(id, editVote);
                if (!result) return NotFound(new { message = "Error 404: El voto no existe." });
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
                if (result == -1) return NotFound(new { message = "Error 404: El voto no existe." });
                return Ok(new { isActive = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error 500: {ex.Message}" });
            }
        }
    }
}
