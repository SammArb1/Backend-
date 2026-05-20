using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IPlanService
    {
        Task<List<Plan>> GetAll();
        Task<Plan?> GetById(Guid id);
        Task<Plan> CreatePlanWithOptions(ApiProyectoWeb.Models.DTOs.CreatePlanDto planDto, string userId);
        Task<Plan?> TransitionState(Guid planId, string userId);
        Task<bool> Edit(Guid id, Plan editPlan);
        Task<int> ChangeStatus(Guid id);
        Task<object> GetByParche(Guid parcheId);
    }
}
