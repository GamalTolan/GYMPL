using GYMBLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MemberShipViewModel>GetAllMemberShips();
        bool RemoveMembership(int membershipId);
        bool CreateMembership(CreateMempershipViewModel model);
        public bool ActivateMembership(int membershipId);
    }
    
}
