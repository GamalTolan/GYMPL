using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.PlanViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;

namespace GYMBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll() ?? [];
            var planViewModels = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            }).ToList();
            return planViewModels;
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null)
                return null;
            var planDetails = new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
            return planDetails;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || plan.IsActive == false)
                return null;
            var updatePlanViewModel = new UpdatePlanViewModel
            {

                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,

            };
            return updatePlanViewModel;
        }

        public bool Activate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null|| HasActiveMemberships(planId))
                return false;
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            _unitOfWork.SaveChanges();
            return true;

        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatePlanViewModel)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || HasActiveMemberships(planId))
                return false;
            plan.Name = updatePlanViewModel.Name;
            plan.Description = updatePlanViewModel.Description;
            plan.DurationDays = updatePlanViewModel.DurationDays;
            plan.Price = updatePlanViewModel.Price;
            plan.IsActive = updatePlanViewModel.IsActive;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            _unitOfWork.SaveChanges();
            return true;
        }


        #region Helper Methods

        private bool HasActiveMemberships(int planId)
        {
            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.PlanId == planId && m.Status == "Active").Any();
            return memberships;
            
        }

    } 
        #endregion
   }
