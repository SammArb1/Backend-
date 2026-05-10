using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _context;

        public PlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAll()
        {
            return await _context.Plans.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Plan?> GetById(Guid id)
        {
            return await _context.Plans.FindAsync(id);
        }

        public async Task<Plan> Create(Plan newPlan)
        {
            _context.Plans.Add(newPlan);
            await _context.SaveChangesAsync();
            return newPlan;
        }

        public async Task<bool> Edit(Guid id, Plan editPlan)
        {
            var objExist = await _context.Plans.FindAsync(id);
            if (objExist == null) return false;

            objExist.title = editPlan.title;
            objExist.description = editPlan.description;
            objExist.dateWindow_start = editPlan.dateWindow_start;
            objExist.dateWindow_end = editPlan.dateWindow_end;
            objExist.state = editPlan.state;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Plans.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
