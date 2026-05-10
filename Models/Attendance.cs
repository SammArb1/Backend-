using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProyectoWeb.Models
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id_attendance { get; set; }

        public Guid id_plan { get; set; }

        public string id_user { get; set; } = string.Empty;

        public string status { get; set; } = "MAYBE"; // YES, NO, MAYBE

        public bool checkedIn { get; set; } = false;

        public int isActive { get; set; } = 1;
    }
}
