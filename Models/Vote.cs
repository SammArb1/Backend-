using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_vote { get; set; }

        public Guid id_plan { get; set; }

        public string id_user { get; set; } = string.Empty;

        public Guid id_option { get; set; }

        public int isActive { get; set; } = 1;
    }
}
