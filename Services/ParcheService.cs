using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class ParcheService : IParcheService
    {
        private readonly ApplicationDbContext _context;

        public ParcheService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Parche>> GetAll()
        {
            return await _context.Parches.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Parche?> GetById(Guid id)
        {
            return await _context.Parches.FindAsync(id);
        }

        public async Task<Parche> Create(Parche newParche)
        {
            _context.Parches.Add(newParche);
            await _context.SaveChangesAsync();
            return newParche;
        }

        public async Task<bool> Edit(Guid id, Parche editParche)
        {
            var objExist = await _context.Parches.FindAsync(id);
            if (objExist == null) return false;

            objExist.name = editParche.name;
            objExist.description = editParche.description;
            objExist.coverUrl = editParche.coverUrl;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Parches.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
