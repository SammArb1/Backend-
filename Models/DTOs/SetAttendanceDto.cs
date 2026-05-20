using System;

namespace ApiProyectoWeb.Models.DTOs
{
    public class SetAttendanceDto
    {
        public Guid PlanId { get; set; }
        public string Status { get; set; } = string.Empty; // "YES", "NO", "MAYBE"
    }
}
