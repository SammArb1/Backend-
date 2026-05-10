using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class ParcheMemberService : IParcheMemberService
    {
        private readonly ApplicationDbContext _context;

        public ParcheMemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ParcheMember>> GetAll()
        {
            return await _context.ParcheMembers.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<ParcheMember?> GetById(Guid id)
        {
            return await _context.ParcheMembers.FindAsync(id);
        }

        public async Task<ParcheMember> Create(ParcheMember newMember)
        {
            _context.ParcheMembers.Add(newMember);
            await _context.SaveChangesAsync();
            return newMember;
        }

        public async Task<bool> Edit(Guid id, ParcheMember editMember)
        {
            var objExist = await _context.ParcheMembers.FindAsync(id);
            if (objExist == null) return false;

            objExist.role = editMember.role;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.ParcheMembers.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
