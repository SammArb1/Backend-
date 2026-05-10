using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_plan { get; set; }

        public Guid id_parche { get; set; }

        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public DateTime dateWindow_start { get; set; }
        public DateTime dateWindow_end { get; set; }

        public string state { get; set; } = "DRAFT"; // DRAFT, VOTING_OPEN, VOTING_CLOSED, SCHEDULED

        public Guid? winningOptionId { get; set; }

        public string createdBy { get; set; } = string.Empty;

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public int isActive { get; set; } = 1;
    }
}
