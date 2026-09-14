using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GYMDAL.Entities;

namespace GYMPL.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(IAccountService accountService ,SignInManager<ApplicationUser> signInManager)
    {
        _accountService = accountService;
        _signInManager = signInManager;
    }
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");  
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if(!ModelState.IsValid)
            return View(model);
        var user= _accountService.ValidateUser(model);

        if(user is null) 
        {
            ModelState.AddModelError("InvalideLogin", "This Account not Found");
            return View(model);

        }
        var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

        if (result.IsNotAllowed)
            ModelState.AddModelError("InvalideLogin","Account is not allowed");
        if (result.IsLockedOut)
            ModelState.AddModelError("InvalideLogin", "Account is Is Locked Out");
        if(result.Succeeded)
          return  RedirectToAction("Index", "Home");
        return View(model);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _signInManager.SignOutAsync().GetAwaiter().GetResult();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}