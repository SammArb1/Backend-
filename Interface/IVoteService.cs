using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IVoteService
    {
        Task<List<Vote>> GetAll();
        Task<Vote?> GetById(Guid id);
        Task<Vote> Create(Vote vote);
        Task<Vote?> CastVote(ApiProyectoWeb.Models.DTOs.CastVoteDto voteDto, string userId);
        Task<bool> Edit(Guid id, Vote editVote);
        Task<int> ChangeStatus(Guid id);
        Task<object> GetByPlan(Guid planId);
    }
}
