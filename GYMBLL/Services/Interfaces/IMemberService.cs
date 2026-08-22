using GYMBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Interfaces
{
    public interface IMemberService
    { 
        IEnumerable<MemberViewModel>GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMemberViewModel);
        bool UpdateMemberDetails(int id,UpdateMemberViewModel model);
        bool RemoveMember(int id);
        MemberViewModel GetMemberDetails(int id);
        HealthRecordViewModel GetHealthRecordDetails(int id);
        UpdateMemberViewModel GetMemberToUpdate(int id);
    }
}
