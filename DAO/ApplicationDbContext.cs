using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.DAO
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Parche> Parches { get; set; }
        public DbSet<ParcheMember> ParcheMembers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanOption> PlanOptions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
