using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAll();
        Task<Attendance?> GetById(Guid id);
        Task<Attendance> Create(Attendance attendance);
        Task<bool> Edit(Guid id, Attendance editAttendance);
        Task<int> ChangeStatus(Guid id);
    }
}
