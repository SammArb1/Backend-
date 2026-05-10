using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IParcheMemberService
    {
        Task<List<ParcheMember>> GetAll();
        Task<ParcheMember?> GetById(Guid id);
        Task<ParcheMember> Create(ParcheMember member);
        Task<bool> Edit(Guid id, ParcheMember editMember);
        Task<int> ChangeStatus(Guid id);
    }
}
