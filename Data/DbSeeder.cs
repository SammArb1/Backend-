using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProyectoWeb.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();

            // 1. Crear usuario por defecto si no existe
            var defaultEmail = "carlos@eia.edu.co";
            var user = await userManager.FindByEmailAsync(defaultEmail);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = defaultEmail,
                    Email = defaultEmail,
                    FullName = "Carlos Demo",
                    Major = "Ingeniería de Sistemas"
                };
                var result = await userManager.CreateAsync(user, "Proyecto1234!");
                if (!result.Succeeded) return; // Si falla la clave u otro error, no creamos más datos
            }

            // 2. Crear parches de prueba si no existen
            if (!context.Parches.Any(p => p.inviteCode == "COD123"))
            {
                var parche1 = new Parche
                {
                    id_parche = Guid.NewGuid(),
                    name = "Tardes de Código ☕",
                    description = "Nos reunimos a programar y tomar tinto.",
                    coverUrl = "https://images.unsplash.com/photo-1517694712202-14dd9538aa97?auto=format&fit=crop&q=80",
                    inviteCode = "COD123",
                    createdAt = DateTime.UtcNow,
                    isActive = 1
                };

                var parche2 = new Parche
                {
                    id_parche = Guid.NewGuid(),
                    name = "Fútbol los Viernes ⚽",
                    description = "Torneo interno de la EIA.",
                    coverUrl = "https://images.unsplash.com/photo-1579952363873-27f3bade9f55?auto=format&fit=crop&q=80",
                    inviteCode = "GOL456",
                    createdAt = DateTime.UtcNow,
                    isActive = 1
                };

                context.Parches.AddRange(parche1, parche2);

                // Asignar a Carlos como OWNER de ambos parches
                context.ParcheMembers.Add(new ParcheMember { id_parche = parche1.id_parche, id_user = user.Id, role = "OWNER", isActive = 1 });
                context.ParcheMembers.Add(new ParcheMember { id_parche = parche2.id_parche, id_user = user.Id, role = "OWNER", isActive = 1 });

                // Crear un plan para el Parche 1
                var plan = new Plan
                {
                    id_plan = Guid.NewGuid(),
                    id_parche = parche1.id_parche,
                    title = "Hackathon de fin de semestre",
                    description = "Vamos a definir dónde nos quedaremos a programar toda la noche.",
                    dateWindow_start = DateTime.UtcNow,
                    dateWindow_end = DateTime.UtcNow.AddDays(7),
                    state = "VOTING_OPEN",
                    createdBy = user.Id,
                    createdAt = DateTime.UtcNow,
                    isActive = 1
                };
                context.Plans.Add(plan);

                // Opciones del plan
                context.PlanOptions.Add(new PlanOption { id_plan_option = Guid.NewGuid(), id_plan = plan.id_plan, place = "Biblioteca", time = "20:00", votesCount = 0, isActive = 1 });
                context.PlanOptions.Add(new PlanOption { id_plan_option = Guid.NewGuid(), id_plan = plan.id_plan, place = "Cafetería", time = "18:00", votesCount = 0, isActive = 1 });

                await context.SaveChangesAsync();
            }
        }
    }
}
