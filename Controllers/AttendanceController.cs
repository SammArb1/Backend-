using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Attendance newAttendance)
        {
            var result = await _service.Create(newAttendance);
            return CreatedAtAction(nameof(GetById), new { id = result.id_attendance }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] Attendance editAttendance)
        {
            var result = await _service.Edit(id, editAttendance);
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
