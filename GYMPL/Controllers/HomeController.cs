using GYMBLL.Services.Interfaces;
using GYMPL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GYMPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticService _analyticService;

        public HomeController(ILogger<HomeController> logger , IAnalyticService analyticService)
        {
            _logger = logger;
            _analyticService = analyticService;
        }

        public IActionResult Index()
        {
            var analytics = _analyticService.GetAnalytics();
            return View(analytics);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
