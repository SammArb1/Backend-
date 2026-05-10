using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class Parche
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_parche { get; set; }

        public string name { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string coverUrl { get; set; } = string.Empty;

        public string inviteCode { get; set; } = string.Empty;

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public int isActive { get; set; } = 1;
    }
}
