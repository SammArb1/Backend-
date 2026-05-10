using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class ParcheMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_parche_member { get; set; }

        public Guid id_parche { get; set; }
        public string id_user { get; set; } = string.Empty;

        public string role { get; set; } = string.Empty; // OWNER, MODERATOR, MEMBER

        public int isActive { get; set; } = 1;
    }
}
