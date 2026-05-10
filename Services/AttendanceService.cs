using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAll()
        {
            return await _context.Attendances.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Attendance?> GetById(Guid id)
        {
            return await _context.Attendances.FindAsync(id);
        }

        public async Task<Attendance> Create(Attendance newAttendance)
        {
            _context.Attendances.Add(newAttendance);
            await _context.SaveChangesAsync();
            return newAttendance;
        }

        public async Task<bool> Edit(Guid id, Attendance editAttendance)
        {
            var objExist = await _context.Attendances.FindAsync(id);
            if (objExist == null) return false;

            objExist.status = editAttendance.status;
            objExist.checkedIn = editAttendance.checkedIn;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Attendances.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }
    }
}
