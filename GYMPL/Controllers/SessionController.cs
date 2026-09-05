using GYMBLL.Services.Classes;
using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        public IActionResult Create()
        {
            LoadCategoryDropDown();
            LoadTrainerDropDown();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadCategoryDropDown();
                LoadTrainerDropDown();
                return View(model);
            }
            var sessions = _sessionService.CreateSession(model);    
            if(sessions)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Failed to create session.");
                LoadCategoryDropDown();
                LoadTrainerDropDown();
                return View(model);
            }
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropDown();


            return View(session);
        }

        [HttpPost]
        public IActionResult Edit(int id, UpdateSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }
            var isUpdated = _sessionService.UpdateSession(id, model);
            if(isUpdated)
                TempData["SuccessMessage"] = "Session updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update session.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction("Index");
            }

            var member = _sessionService.GetSessionById(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }

            ViewBag.SessionId = id;
            return View(member);
        }

        [HttpPost]
        public IActionResult DeleteConfirm([FromForm] int id)
        {
            bool isDeleted = _sessionService.RemoveSession(id);
            if (isDeleted)
                TempData["SuccessMessage"] = "Session deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete session. Please try again.";
            return RedirectToAction("Index");
        }
        #region Helper Methods

        public void LoadCategoryDropDown()
        {
            var categories = _sessionService.GetCategoriesDropdown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
        public void LoadTrainerDropDown()
        {
            var trainers = _sessionService.GetTrainersDropdown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion
    }
}
