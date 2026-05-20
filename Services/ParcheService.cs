using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class ParcheService : IParcheService
    {
        private readonly ApplicationDbContext _context;

        public ParcheService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Parche>> GetAll()
        {
            return await _context.Parches.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Parche?> GetById(Guid id)
        {
            return await _context.Parches.FindAsync(id);
        }

        public async Task<Parche> Create(Parche newParche, string userId)
        {
            newParche.id_parche = Guid.NewGuid();
            newParche.inviteCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            newParche.createdAt = DateTime.UtcNow;
            newParche.isActive = 1;
            
            _context.Parches.Add(newParche);

            var member = new ParcheMember
            {
                id_parche = newParche.id_parche,
                id_user = userId,
                role = "OWNER",
                isActive = 1
            };
            _context.ParcheMembers.Add(member);

            await _context.SaveChangesAsync();
            return newParche;
        }

        public async Task<Parche?> Join(string inviteCode, string userId)
        {
            var parche = await _context.Parches.FirstOrDefaultAsync(p => p.inviteCode == inviteCode && p.isActive == 1);
            if (parche == null) throw new KeyNotFoundException("Error 404: El código de parche ingresado no existe o caducó.");

            var existingMember = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parche.id_parche && m.id_user == userId && m.isActive == 1);
            if (existingMember != null) throw new InvalidOperationException("Error 400: Ya formas parte de este parche.");

            var newMember = new ParcheMember
            {
                id_parche = parche.id_parche,
                id_user = userId,
                role = "MEMBER",
                isActive = 1
            };
            _context.ParcheMembers.Add(newMember);
            await _context.SaveChangesAsync();

            return parche;
        }

        public async Task<bool> Edit(Guid id, Parche editParche)
        {
            var objExist = await _context.Parches.FindAsync(id);
            if (objExist == null) return false;

            objExist.name = editParche.name;
            objExist.description = editParche.description;
            objExist.coverUrl = editParche.coverUrl;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Parches.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }

        public async Task<object> GetMyParches(string userId)
        {
            var myMemberships = await _context.ParcheMembers.Where(pm => pm.id_user == userId && pm.isActive == 1).ToListAsync();
            var parcheIds = myMemberships.Select(m => m.id_parche).ToList();
            
            var parches = await _context.Parches.Where(p => parcheIds.Contains(p.id_parche) && p.isActive == 1).ToListAsync();
            
            var allMembers = await _context.ParcheMembers.Where(pm => parcheIds.Contains(pm.id_parche) && pm.isActive == 1).ToListAsync();
            
            return parches.Select(p => new {
                id_parche = p.id_parche,
                name = p.name,
                description = p.description,
                coverUrl = p.coverUrl,
                inviteCode = p.inviteCode,
                createdAt = p.createdAt,
                members = allMembers.Where(m => m.id_parche == p.id_parche).Select(m => new {
                    userId = m.id_user,
                    role = m.role
                }).ToList()
            }).ToList();
        }

        public async Task<bool> Leave(Guid parcheId, string userId)
        {
            var member = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parcheId && m.id_user == userId && m.isActive == 1);
            if (member == null) return false;
            
            member.isActive = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetMemberRole(Guid parcheId, string targetUserId, string newRole, string requesterUserId)
        {
            var requester = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parcheId && m.id_user == requesterUserId && m.isActive == 1);
            if (requester == null || requester.role != "OWNER")
                throw new InvalidOperationException("Error 403: Solo el OWNER puede cambiar roles.");

            var target = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parcheId && m.id_user == targetUserId && m.isActive == 1);
            if (target == null) throw new KeyNotFoundException("Error 404: El usuario no pertenece al parche.");

            target.role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMember(Guid parcheId, string targetUserId, string requesterUserId)
        {
            var requester = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parcheId && m.id_user == requesterUserId && m.isActive == 1);
            if (requester == null || (requester.role != "OWNER" && requester.role != "MODERATOR"))
                throw new InvalidOperationException("Error 403: No tienes permisos para eliminar miembros.");

            if (targetUserId == requesterUserId)
                throw new InvalidOperationException("Error 400: Usa la opción de salir del parche en lugar de eliminarte a ti mismo.");

            var target = await _context.ParcheMembers.FirstOrDefaultAsync(m => m.id_parche == parcheId && m.id_user == targetUserId && m.isActive == 1);
            if (target == null) throw new KeyNotFoundException("Error 404: El usuario no pertenece al parche.");

            target.isActive = 0;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
