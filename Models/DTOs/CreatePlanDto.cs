using System;
using System.Collections.Generic;

namespace ApiProyectoWeb.Models.DTOs
{
    public class PlanOptionDto
    {
        public string Place { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
    }

    public class CreatePlanDto
    {
        public Guid ParcheId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateWindowStart { get; set; }
        public DateTime DateWindowEnd { get; set; }
        public List<PlanOptionDto> Options { get; set; } = new List<PlanOptionDto>();
    }
}
