using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.ViewModels.MemberSessionViewModels
{
    public class BookingViewModel
    {
        // ===== BOOKING INFO =====
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsAttended { get; set; }

        // ===== MEMBER INFO =====
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string? MemberPhoto { get; set; }
        public string? MemberPhone { get; set; }
        public string? MemberEmail { get; set; }

        // ===== SESSION INFO =====
        public int SessionId { get; set; }
        public string? SessionCategory { get; set; }
        public string? SessionTrainer { get; set; }
        public DateTime? SessionStartDate { get; set; }
        public DateTime? SessionEndDate { get; set; }
    }
}
