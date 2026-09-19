using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MemberSessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPL.Controllers
{
    [Authorize]
    public class MemberSessionController : Controller
    {
        private readonly IMemberSessionService _memberSessionService;
        private readonly ISessionService _sessionService;

        public MemberSessionController(
            IMemberSessionService memberSessionService,
            ISessionService sessionService)
        {
            _memberSessionService = memberSessionService;
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions().ToList();

            var model = new SessionScheduleViewModel
            {
                UpcomingSessions = sessions.Where(s => s.Status == "Upcoming"),
                OngoingSessions = sessions.Where(s => s.Status == "Ongoing"),
                CompletedSessions = sessions.Where(s => s.Status == "Completed")
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetMembersForUpcomingSession(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var session = _sessionService.GetSessionById(sessionId);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var bookings = _memberSessionService.GetBookingsForSession(sessionId);

            ViewBag.SessionId = sessionId;
            ViewBag.SessionCategory = session.CategoryName;
            ViewBag.SessionDate = session.StartDate;

            return View(bookings);
        }

      
        [HttpGet]
        public IActionResult GetMembersForOngoingSessions(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var session = _sessionService.GetSessionById(sessionId);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var bookings = _memberSessionService.GetBookingsForSession(sessionId);

            ViewBag.SessionId = sessionId;
            ViewBag.SessionCategory = session.CategoryName;
            ViewBag.SessionDate = session.StartDate;

            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create(int sessionId)
        {
            if (sessionId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var session = _sessionService.GetSessionById(sessionId);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var availableMembers = _memberSessionService.GetAvailableMembers(sessionId);

            var model = new CreateBookingViewModel
            {
                SessionId = sessionId,
                Members = availableMembers.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
            };

            ViewBag.SessionCategory = session.CategoryName;
            ViewBag.SessionDate = session.StartDate;
            ViewBag.AvailableSlots = session.AvailableSlots;

            return View(model);
        }

        
        [HttpPost]
        public IActionResult Create(CreateBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = _memberSessionService.GetAvailableMembers(model.SessionId)
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name
                    });

                return View(model);
            }

            var result = _memberSessionService.CreateBooking(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Booking created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking. Member may already be booked or session is full.";
            }

            return RedirectToAction(nameof(GetMembersForUpcomingSession),
                new { sessionId = model.SessionId });
        }

        [HttpPost]
        public IActionResult Cancel(int id, int sessionId)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid booking ID.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var result = _memberSessionService.CancelBooking(id);

            if (result)
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            else
                TempData["ErrorMessage"] = "Failed to cancel booking. The session may have already started.";

            return RedirectToAction(nameof(GetMembersForUpcomingSession),
                new { sessionId });
        }

       
        [HttpPost]
        public IActionResult MarkAttendance(int id, int sessionId)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid booking ID.";
                return RedirectToAction("Index", "SessionSchedule");
            }

            var result = _memberSessionService.MarkAttendance(id);

            if (result)
                TempData["SuccessMessage"] = "Attendance marked successfully.";
            else
                TempData["ErrorMessage"] = "Failed to mark attendance. The session may not be ongoing.";

            return RedirectToAction(nameof(GetMembersForOngoingSessions),
                new { sessionId });
        }
    }
}
