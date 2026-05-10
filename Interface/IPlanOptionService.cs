using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IPlanOptionService
    {
        Task<List<PlanOption>> GetAll();
        Task<PlanOption?> GetById(Guid id);
        Task<PlanOption> Create(PlanOption option);
        Task<bool> Edit(Guid id, PlanOption editOption);
        Task<int> ChangeStatus(Guid id);
    }
}
