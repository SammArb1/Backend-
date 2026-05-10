using Microsoft.AspNetCore.Identity;

namespace ApiProyectoWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int isActive { get; set; } = 1;
    }
}
