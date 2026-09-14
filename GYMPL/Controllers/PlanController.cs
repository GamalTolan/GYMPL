using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYMPL.Controllers;

[Authorize]

public class PlanController : Controller
{
    private readonly IPlanService _planService;

    public PlanController(IPlanService planService)
    {
        _planService = planService;
    }
    public IActionResult Index()
    {
        var plans = _planService.GetAllPlans();
        return View(plans);
    }
    public IActionResult Details(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid plan ID.";
            return RedirectToAction(nameof(Index));
        }
        var plan = _planService.GetPlanById(id);
        if (plan == null)
        {
            TempData["ErrorMessage"] = "Plan not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

    public IActionResult Edit(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid plan ID.";
            return RedirectToAction(nameof(Index));
        }
        var plan = _planService.GetPlanToUpdate(id);
        if (plan == null)
        {
            TempData["ErrorMessage"] = "Plan not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

    [HttpPost]
    public IActionResult Edit([FromRoute] int id,  UpdatePlanViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid plan ID.";
            return RedirectToAction(nameof(Index));
        }
        var result = _planService.UpdatePlan(id, model);
        if (result)
            TempData["SuccessMessage"] = "Plan updated successfully.";
        else
            TempData["ErrorMessage"] = "Failed to update plan.";

        return RedirectToAction(nameof(Index));
        
    }
    [HttpPost]
    public IActionResult Activate(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Invalid plan ID.";
            return RedirectToAction(nameof(Index));
        }
        var result = _planService.Activate(id);
        if (result)
            TempData["SuccessMessage"] = "Plan status updated successfully.";
        else
            TempData["ErrorMessage"] = "Failed to update plan status.";
        return RedirectToAction(nameof(Index));
    }
}
