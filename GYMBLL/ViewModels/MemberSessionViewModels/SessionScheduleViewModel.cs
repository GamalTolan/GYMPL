using GYMBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.ViewModels.MemberSessionViewModels
{
    public class SessionScheduleViewModel
    {
        public IEnumerable<SessionViewModel> UpcomingSessions { get; set; } = [];
        public IEnumerable<SessionViewModel> OngoingSessions { get; set; } = [];
        public IEnumerable<SessionViewModel> CompletedSessions { get; set; } = [];
    }
}
