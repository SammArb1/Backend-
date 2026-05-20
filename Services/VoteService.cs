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

        public async Task<Vote?> CastVote(ApiProyectoWeb.Models.DTOs.CastVoteDto voteDto, string userId)
        {
            var plan = await _context.Plans.FindAsync(voteDto.PlanId);
            if (plan == null || plan.state != "VOTING_OPEN" || plan.isActive == 0) return null;

            var existingVote = await _context.Votes.FirstOrDefaultAsync(v => v.id_plan == voteDto.PlanId && v.id_user == userId && v.isActive == 1);
            if (existingVote != null)
            {
                existingVote.id_option = voteDto.OptionId;
                await _context.SaveChangesAsync();
                return existingVote;
            }

            var newVote = new Vote
            {
                id_vote = Guid.NewGuid(),
                id_plan = voteDto.PlanId,
                id_user = userId,
                id_option = voteDto.OptionId,
                isActive = 1
            };
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

        public async Task<object> GetByPlan(Guid planId)
        {
            var votes = await _context.Votes.Where(v => v.id_plan == planId && v.isActive == 1).ToListAsync();
            return votes.Select(v => new {
                id_vote = v.id_vote,
                id_plan = v.id_plan,
                id_user = v.id_user,
                id_option = v.id_option
            }).ToList();
        }
    }
}
