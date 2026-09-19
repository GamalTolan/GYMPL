using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.ViewModels.MemberSessionViewModels
{
    public class AttendanceViewModel
    {
        public int BookingId { get; set; }              
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public bool IsAttended { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
