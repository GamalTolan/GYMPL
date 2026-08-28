using GYMBLL.Services.Classes;
using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GYMPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        public IActionResult MemberDetails(int id) 
        { 
            var member = _memberService.GetMemberDetails(id);
            if(member is null)
            {
                return RedirectToAction("Index");
            }
            return View(member);
        }
        public IActionResult HealthRecordDetails(int id) 
        { 
            var member = _memberService.GetHealthRecordDetails(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }
            return View(member);
        }
        public IActionResult CreateMember()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Please fill in all required fields.");
                return RedirectToAction(nameof(CreateMember) , model);
            }
            bool isCreated = _memberService.CreateMember(model);
            if (isCreated)
                TempData["SuccessMessage"] = "Member created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MemberEdit(int id)
        {
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";

                return RedirectToAction("Index");
            }
            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit(int id ,UpdateMemberViewModel model)
        {
            f(!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = _memberService.UpdateMemberDetails(id, model);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update member. Please try again.";

            return View(model);
        }

        public IActionResult Delete ([FromRoute]int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID.";
                return RedirectToAction("Index");
            }
            
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = id;
            return View(member);
        }

        [HttpPost]
        public IActionResult DeleteConfirm([FromForm]int id)
        {
            bool isDeleted = _memberService.RemoveMember(id);
            if (isDeleted)
                TempData["SuccessMessage"] = "Member deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete member. Please try again.";
            return RedirectToAction("Index");
        }
    }
}
