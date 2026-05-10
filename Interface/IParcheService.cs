using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IParcheService
    {
        Task<List<Parche>> GetAll();
        Task<Parche?> GetById(Guid id);
        Task<Parche> Create(Parche parche);
        Task<bool> Edit(Guid id, Parche editParche);
        Task<int> ChangeStatus(Guid id);
    }
}
