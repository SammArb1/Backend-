using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class VoteService : IVoteService
    {
        private readonly ApplicationDbContext _context;

        public VoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vote>> GetAll()
        {
            return await _context.Votes.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Vote?> GetById(Guid id)
        {
            return await _context.Votes.FindAsync(id);
        }

        public async Task<Vote> Create(Vote newVote)
        {
            _context.Votes.Add(newVote);
            await _context.SaveChangesAsync();
            return newVote;
        }

        public async Task<bool> Edit(Guid id, Vote editVote)
        {
            var objExist = await _context.Votes.FindAsync(id);
            if (objExist == null) return false;

            objExist.id_option = editVote.id_option;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Votes.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
