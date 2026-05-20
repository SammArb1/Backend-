using ApiProyectoWeb.Models;

namespace ApiProyectoWeb.Interface
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAll();
        Task<Attendance?> GetById(Guid id);
        Task<Attendance> Create(Attendance attendance);
        Task<Attendance?> SetAttendance(ApiProyectoWeb.Models.DTOs.SetAttendanceDto dto, string userId);
        Task<Attendance?> CheckIn(Guid planId, string userId);
        Task<bool> Edit(Guid id, Attendance editAttendance);
        Task<int> ChangeStatus(Guid id);
        Task<object> GetByPlan(Guid planId);
    }
}
