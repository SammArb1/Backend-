using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IPlanService
    {
        Task<List<Plan>> GetAll();
        Task<Plan?> GetById(Guid id);
        Task<Plan> Create(Plan plan);
        Task<bool> Edit(Guid id, Plan editPlan);
        Task<int> ChangeStatus(Guid id);
    }
}
