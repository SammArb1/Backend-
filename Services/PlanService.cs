using ApiProyectoWeb.DAO;
using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProyectoWeb.Services
{
    public class PlanService : IPlanService
    {
        private readonly ApplicationDbContext _context;

        public PlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Plan>> GetAll()
        {
            return await _context.Plans.Where(p => p.isActive == 1).ToListAsync();
        }

        public async Task<Plan?> GetById(Guid id)
        {
            return await _context.Plans.FindAsync(id);
        }

        public async Task<Plan> CreatePlanWithOptions(ApiProyectoWeb.Models.DTOs.CreatePlanDto planDto, string userId)
        {
            var newPlan = new Plan
            {
                id_plan = Guid.NewGuid(),
                id_parche = planDto.ParcheId,
                title = planDto.Title,
                description = planDto.Description,
                dateWindow_start = planDto.DateWindowStart,
                dateWindow_end = planDto.DateWindowEnd,
                state = "DRAFT",
                createdBy = userId,
                createdAt = DateTime.UtcNow,
                isActive = 1
            };
            
            _context.Plans.Add(newPlan);

            foreach (var opt in planDto.Options)
            {
                var newOption = new PlanOption
                {
                    id_plan_option = Guid.NewGuid(),
                    id_plan = newPlan.id_plan,
                    place = opt.Place,
                    time = opt.Time,
                    votesCount = 0,
                    isActive = 1
                };
                _context.PlanOptions.Add(newOption);
            }

            await _context.SaveChangesAsync();
            return newPlan;
        }

        public async Task<Plan?> TransitionState(Guid planId, string userId)
        {
            var plan = await _context.Plans.FindAsync(planId);
            if (plan == null) return null;

            if (plan.state == "DRAFT") plan.state = "VOTING_OPEN";
            else if (plan.state == "VOTING_OPEN") 
            {
                plan.state = "VOTING_CLOSED";
                
                var options = await _context.PlanOptions.Where(o => o.id_plan == planId).ToListAsync();
                var votes = await _context.Votes.Where(v => v.id_plan == planId).ToListAsync();
                
                foreach(var opt in options) {
                    opt.votesCount = votes.Count(v => v.id_option == opt.id_plan_option);
                }

                if (options.Any()) {
                    var winner = options.OrderByDescending(o => o.votesCount).ThenBy(o => o.time).First();
                    plan.winningOptionId = winner.id_plan_option;
                }
            }
            else if (plan.state == "VOTING_CLOSED") plan.state = "SCHEDULED";
            else if (plan.state == "SCHEDULED") plan.state = "COMPLETED";

            await _context.SaveChangesAsync();
            return plan;
        }

        public async Task<bool> Edit(Guid id, Plan editPlan)
        {
            var objExist = await _context.Plans.FindAsync(id);
            if (objExist == null) return false;

            objExist.title = editPlan.title;
            objExist.description = editPlan.description;
            objExist.dateWindow_start = editPlan.dateWindow_start;
            objExist.dateWindow_end = editPlan.dateWindow_end;
            objExist.state = editPlan.state;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ChangeStatus(Guid id)
        {
            var objExist = await _context.Plans.FindAsync(id);
            if (objExist == null) return -1;

            objExist.isActive = objExist.isActive == 0 ? 1 : 0;

            await _context.SaveChangesAsync();
            return objExist.isActive;
        }

        public async Task<object> GetByParche(Guid parcheId)
        {
            var plans = await _context.Plans.Where(p => p.id_parche == parcheId && p.isActive == 1).ToListAsync();
            var planIds = plans.Select(p => p.id_plan).ToList();
            var options = await _context.PlanOptions.Where(o => planIds.Contains(o.id_plan) && o.isActive == 1).ToListAsync();
            var votes = await _context.Votes.Where(v => planIds.Contains(v.id_plan) && v.isActive == 1).ToListAsync();

            return plans.Select(p => new {
                id_plan = p.id_plan,
                id_parche = p.id_parche,
                title = p.title,
                description = p.description,
                dateWindow_start = p.dateWindow_start,
                dateWindow_end = p.dateWindow_end,
                state = p.state,
                winningOptionId = p.winningOptionId,
                createdBy = p.createdBy,
                createdAt = p.createdAt,
                options = options.Where(o => o.id_plan == p.id_plan).Select(o => new {
                    id_plan_option = o.id_plan_option,
                    place = o.place,
                    time = o.time,
                    votesCount = p.state == "VOTING_OPEN" ? votes.Count(v => v.id_option == o.id_plan_option) : o.votesCount
                }).ToList()
            }).ToList();
        }
    }
}
