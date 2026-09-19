using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMBLL.ViewModels.MemberSessionViewModels;

public class CreateBookingViewModel
{
    [Required(ErrorMessage = "Member is required")]
    [Display(Name = "Member")]
    public int MemberId { get; set; }

    [Required]
    [Display(Name = "Session")]
    public int SessionId { get; set; }

    public IEnumerable<SelectListItem>? Members { get; set; }
}
