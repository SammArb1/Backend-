using System;

namespace ApiProyectoWeb.Models.DTOs
{
    public class CastVoteDto
    {
        public Guid PlanId { get; set; }
        public Guid OptionId { get; set; }
    }
}
