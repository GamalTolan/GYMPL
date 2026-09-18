using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IPlanService _planService;
        private readonly IMemberService _memberService;

        public MembershipController(IMembershipService membershipService, IPlanService planService , IMemberService memberService)
        {
            _membershipService = membershipService;
            _planService = planService;
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberShips();
            return View(memberships);
        }

       
        public IActionResult Create()
        {
            ViewBag.Members = _memberService.GetAllMembers()
              .Select(m => new SelectListItem
              {
                  Value = m.Id.ToString(),
                  Text = m.Name
              }).ToList();

            ViewBag.Plans = _planService.GetAllPlans()
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Name} - {p.Price} EGP ({p.DurationDays} Days)"
                }).ToList();

            return View();
        }

        
        [HttpPost]
        public IActionResult Create(CreateMempershipViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _membershipService.CreateMembership(model);

            if (result)
                TempData["SuccessMessage"] = "Membership created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create membership. Please check the plan and try again.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid membership ID.";
                return RedirectToAction(nameof(Index));
            }

            var result = _membershipService.ActivateMembership(id);

            if (result)
                TempData["SuccessMessage"] = "Membership activated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to activate membership. It may already be active or the plan is not available.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid membership ID.";
                return RedirectToAction(nameof(Index));
            }

            var result = _membershipService.RemoveMembership(id);

            if (result)
                TempData["SuccessMessage"] = "Membership deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete membership. It may still be active.";

            return RedirectToAction(nameof(Index));
        }
    }
}