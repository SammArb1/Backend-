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

        public async Task<Attendance?> SetAttendance(ApiProyectoWeb.Models.DTOs.SetAttendanceDto dto, string userId)
        {
            var plan = await _context.Plans.FindAsync(dto.PlanId);
            if (plan == null || (plan.state != "SCHEDULED" && plan.state != "VOTING_CLOSED")) return null;

            var existing = await _context.Attendances.FirstOrDefaultAsync(a => a.id_plan == dto.PlanId && a.id_user == userId && a.isActive == 1);
            if (existing != null)
            {
                existing.status = dto.Status;
                await _context.SaveChangesAsync();
                return existing;
            }

            var newAttendance = new Attendance
            {
                id_attendance = Guid.NewGuid(),
                id_plan = dto.PlanId,
                id_user = userId,
                status = dto.Status,
                checkedIn = false,
                isActive = 1
            };
            _context.Attendances.Add(newAttendance);
            await _context.SaveChangesAsync();
            return newAttendance;
        }

        public async Task<Attendance?> CheckIn(Guid planId, string userId)
        {
            var plan = await _context.Plans.FindAsync(planId);
            if (plan == null || plan.state != "SCHEDULED") return null;

            // En una app real, aquí se verifica si está en la ventana de tiempo.
            // if(DateTime.UtcNow < plan.dateWindow_start || DateTime.UtcNow > plan.dateWindow_end) return null;

            var existing = await _context.Attendances.FirstOrDefaultAsync(a => a.id_plan == planId && a.id_user == userId && a.isActive == 1);
            if (existing != null)
            {
                if (existing.checkedIn) return null; // Already checked in
                existing.checkedIn = true;
                existing.status = "YES";
                await _context.SaveChangesAsync();
                return existing;
            }

            var newAttendance = new Attendance
            {
                id_attendance = Guid.NewGuid(),
                id_plan = planId,
                id_user = userId,
                status = "YES",
                checkedIn = true,
                isActive = 1
            };
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

        public async Task<object> GetByPlan(Guid planId)
        {
            var attendances = await _context.Attendances.Where(a => a.id_plan == planId && a.isActive == 1).ToListAsync();
            return attendances.Select(a => new {
                id_attendance = a.id_attendance,
                id_plan = a.id_plan,
                id_user = a.id_user,
                status = a.status,
                checkedIn = a.checkedIn
            }).ToList();
        }
    }
}
