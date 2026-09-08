using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MemberViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;

namespace GYMBLL.Services.Classes;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public MemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public bool CreateMember(CreateMemberViewModel createMemberViewModel)
    {
        try
        {
            if (IsEmailUnique(createMemberViewModel.Email))
            {
                return false;
            }
            if (IsPhoneNumberUnique(createMemberViewModel.PhoneNumber))
            {
                return false;
            }
            var member = new Member
            {
                Name = createMemberViewModel.Name,
                Photo = createMemberViewModel.Photo?.ToString(),
                Email = createMemberViewModel.Email,
                PhoneNumber = createMemberViewModel.PhoneNumber,
                Gender = createMemberViewModel.Gender,
                Address = new Address
                {
                    BuildingNumber = createMemberViewModel.BuildingNumber,
                    Street = createMemberViewModel.Street,
                    City = createMemberViewModel.City,
                },

                DateOfBirth = createMemberViewModel.DateOfBirth,
                HealthRecord = new HealthRecord
                {
                    Hight = createMemberViewModel.HealthRecord.Hight,
                    Weight = createMemberViewModel.HealthRecord.Weight,
                    BloodType = createMemberViewModel.HealthRecord.BloodType,
                    Note = createMemberViewModel.HealthRecord.Note

                }

            };
            _unitOfWork.GetRepository<Member>().Add(member);
            _unitOfWork.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool RemoveMember(int id)
    {
        var member = _unitOfWork.GetRepository<Member>().GetById(id);
        if (member == null)
            return false;

        var activeBooking = _unitOfWork.GetRepository<Booking>().GetAll(x => x.MemberId == id && x.Session.StartDate > DateOnly.FromDateTime(DateTime.UtcNow));
        if (activeBooking.Any())
        {
            return false;
        }

        var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == id).ToList();
        try
        {
            if (memberships.Any())
            {
                foreach (var membership in memberships)
                {
                    _unitOfWork.GetRepository<Membership>().Delete(membership);
                }
            }
            _unitOfWork.GetRepository<Member>().Delete(member);
            _unitOfWork.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public IEnumerable<MemberViewModel> GetAllMembers()
    {
        var members = _unitOfWork.GetRepository<Member>().GetAll() ?? [];
        var memberViewModels = members.Select(m => new MemberViewModel
        {
            Id = m.Id,
            Photo = m.Photo,
            Name = m.Name,
            Email = m.Email,
            PhoneNumber = m.PhoneNumber,
            DateOfBirth = m.DateOfBirth.ToShortDateString(),
            Gender = m.Gender.ToString(),


        }).ToList();
        return memberViewModels;
    }

    public HealthRecordViewModel GetHealthRecordDetails(int id)
    {
        var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
        if (memberHealthRecord == null)
            return null;
        var healthRecordViewModel = new HealthRecordViewModel
        {
            Weight = memberHealthRecord.Weight,
            Hight = memberHealthRecord.Hight,
            BloodType = memberHealthRecord.BloodType,
            Note = memberHealthRecord.Note
        };
        return healthRecordViewModel;
    }

    public MemberViewModel GetMemberDetails(int id)
    {
        var member = _unitOfWork.GetRepository<Member>().GetById(id);

        if (member == null)
            return null;

        var memberViewModel = new MemberViewModel
        {
            Id = member.Id,
            Photo = member.Photo,
            Name = member.Name,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            DateOfBirth = member.DateOfBirth.ToShortDateString(),
            Address = FormatAddress(member.Address),
            Gender = member.Gender.ToString(),



        };
        var activeMemberShip = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();
        if (activeMemberShip is not null)
        {
            var activePlan = _unitOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);
            memberViewModel.PlanName = activePlan.Name;
            memberViewModel.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
            memberViewModel.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();


        }
        return memberViewModel;
    }

    public UpdateMemberViewModel GetMemberToUpdate(int id)
    {
        var member = _unitOfWork.GetRepository<Member>().GetById(id);
        if (member is null)
            return null;
        var updateMemberViewModel = new UpdateMemberViewModel
        {
            Id = member.Id,
            Name = member.Name,
            Photo = member.Photo,
            Email = member.Email,
            Gender = member.Gender,
            PhoneNumber = member.PhoneNumber,
            BuildingNumber = member.Address.BuildingNumber,
            Street = member.Address.Street,
            City = member.Address.City,

        };
        return updateMemberViewModel;
    }

    public bool UpdateMemberDetails(int id, UpdateMemberViewModel model)
    {


        var emalExist = _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == model.Email && m.Id != id);
        var phoneExist = _unitOfWork.GetRepository<Member>().GetAll(m => m.PhoneNumber == model.PhoneNumber && m.Id != id);

        if (emalExist.Any() || phoneExist.Any())
        {
            return false;
        }
        var member = _unitOfWork.GetRepository<Member>().GetById(id);

        member.Address.BuildingNumber = model.BuildingNumber;
        member.Address.Street = model.Street;
        member.Address.City = model.City;
        member.Name = model.Name;
        member.Email = model.Email;
        member.PhoneNumber = model.PhoneNumber;
        member.UpdatedAt = DateTime.Now;

        _unitOfWork.GetRepository<Member>().Update(member);
        _unitOfWork.SaveChanges();
        return true;
    }


    #region Helper Methods
    private string FormatAddress(Address address)
    {
        if (address == null)
            return "N/A";
        return $"{address.BuildingNumber},{address.Street}, {address.City}";
    }

    private bool IsEmailUnique(string email)
    {
        var existingMember = _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email);
        return existingMember is not null && existingMember.Any();
    }

    private bool IsPhoneNumberUnique(string phoneNumber)
    {
        var existingMember = _unitOfWork.GetRepository<Member>().GetAll(m => m.PhoneNumber == phoneNumber);
        return existingMember is not null && existingMember.Any();
    }

    #endregion
}
