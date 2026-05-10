using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _parcheService.Create(newParche);
            return CreatedAtAction(nameof(GetById), new { id = result.id_parche }, result);
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
    }
}
