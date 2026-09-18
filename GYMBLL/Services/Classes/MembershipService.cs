using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MembershipViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;
using System.Numerics;

namespace GYMBLL.Services.Classes;

public class MembershipService : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork;

    public MembershipService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public bool ActivateMembership(int membershipId)
    {
        try
        {
            var membership = _unitOfWork.GetRepository<Membership>().GetById(membershipId);

            if (membership is null)
                return false;

            if (membership.EndDate >= DateTime.Now)
                return false;

            var plan = _unitOfWork.GetRepository<Plan>().GetById(membership.PlanId);
            if (plan is null || plan.IsActive == false)
                return false;

            membership.EndDate = DateTime.Now.AddDays(plan.DurationDays);
            membership.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Membership>().Update(membership);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool CreateMembership(CreateMempershipViewModel model)
    {
        var plan = _unitOfWork.GetRepository<Plan>().GetById(model.PlanId);

        if (plan is null)
            return false;

        try
        {
            var membership = new Membership
            {
                PlanId = model.PlanId,
                MemberId = model.MemberId,
                EndDate = DateTime.Now.AddDays(plan.DurationDays)

            };
            _unitOfWork.GetRepository<Membership>().Add(membership);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {

            return false;
        }
    }

    public IEnumerable<MemberShipViewModel> GetAllMemberShips()
    {
        var memberships = _unitOfWork.GetRepository<Membership>().GetAll().ToList();

        if (!memberships.Any())
            return [];
        var membershipViewModel = memberships.Select(m =>
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(m.MemberId);
            var plan = _unitOfWork.GetRepository<Plan>().GetById(m.PlanId);
            return new MemberShipViewModel
            {
                Id = m.Id,
                MemberName = member.Name,
                PlanName = plan.Name,
                StartDate = m.CreatedAt,
                EndDate = m.EndDate,
                Status = m.Status

            };

        }).ToList();
        return membershipViewModel;
    }

    public bool RemoveMembership(int membershipId)
    {
        var membership = _unitOfWork.GetRepository<Membership>().GetById(membershipId);
        if (membership is null)
            return false;
        if (membership.EndDate >= DateTime.Now)
            return false;

        _unitOfWork.GetRepository<Membership>().Delete(membership);
        return _unitOfWork.SaveChanges() > 0;

    }
}
