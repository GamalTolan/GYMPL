using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage = "Weight must be grater than 0")]
        public decimal Weight { get; set; }
        [Range(0.1, 300, ErrorMessage = "Height must be grater than 0")]
        public decimal Hight { get; set; }
        [Required(ErrorMessage = "Blood Type is required")]
        [StringLength(3, ErrorMessage = "Blood type must be 3 characters or less")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
