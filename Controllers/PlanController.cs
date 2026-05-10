using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Plan newPlan)
        {
            var result = await _service.Create(newPlan);
            return CreatedAtAction(nameof(GetById), new { id = result.id_plan }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Plan editPlan)
        {
            var result = await _service.Edit(id, editPlan);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            var result = await _service.ChangeStatus(id);
            if (result == -1) return NotFound();
            return Ok(new { isActive = result });
        }
    }
}
