using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class PlanOptionService : IPlanOptionService
    {
        private readonly ApplicationDbContext _context;

        public PlanOptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanOption>> GetAll()
        {
            return await _context.PlanOptions.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<PlanOption?> GetById(Guid id)
        {
            return await _context.PlanOptions.FindAsync(id);
        }

        public async Task<PlanOption> Create(PlanOption newOption)
        {
            _context.PlanOptions.Add(newOption);
            await _context.SaveChangesAsync();
            return newOption;
        }

        public async Task<bool> Edit(Guid id, PlanOption editOption)
        {
            var objExist = await _context.PlanOptions.FindAsync(id);
            if (objExist == null) return false;

            objExist.place = editOption.place;
            objExist.time = editOption.time;
            objExist.votesCount = editOption.votesCount;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.PlanOptions.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
