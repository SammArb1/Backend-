using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IParcheService
    {
        Task<List<Parche>> GetAll();
        Task<Parche?> GetById(Guid id);
        Task<Parche> Create(Parche parche, string userId);
        Task<Parche?> Join(string inviteCode, string userId);
        Task<bool> Edit(Guid id, Parche editParche);
        Task<int> ChangeStatus(Guid id);
        Task<object> GetMyParches(string userId);
        Task<bool> Leave(Guid parcheId, string userId);
        Task<bool> SetMemberRole(Guid parcheId, string targetUserId, string newRole, string requesterUserId);
        Task<bool> RemoveMember(Guid parcheId, string targetUserId, string requesterUserId);
    }
}
