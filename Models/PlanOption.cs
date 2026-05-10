using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class PlanOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_plan_option { get; set; }

        public Guid id_plan { get; set; }

        public string place { get; set; } = string.Empty;

        public string time { get; set; } = string.Empty;

        public int votesCount { get; set; } = 0;

        public int isActive { get; set; } = 1;
    }
}
