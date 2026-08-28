using GYMBLL.Services.Classes;
using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.TrainerViewModels;
using GYMDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GYMPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public IActionResult TrainerDetails(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        public IActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Please fill in all required fields.");
                return RedirectToAction(nameof(CreateTrainer), model);
            }

            bool isCreated = _trainerService.CreateTrainer(model);

            if (isCreated)
                TempData["SuccessMessage"] = "Trainer created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create trainer. Please try again.";

            return RedirectToAction(nameof(Index));


        }
        public IActionResult TrainerEdit(int id)
        {
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public IActionResult TrainerEdit(int id, UpdateTrainerViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = _trainerService.UpdateTrainer(id, model);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update trainer. Please try again.";

            return View(model);
        }

        public IActionResult Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";
                return RedirectToAction("Index");
            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View(trainer);
        }

        [HttpPost]
        public IActionResult DeleteTrainer([FromForm] int id)
        {

            bool isDeleted = _trainerService.RemoveTrainer(id);
            if (isDeleted)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete trainer. Please try again.";
            return RedirectToAction(nameof(Index));
        }

    }
}
