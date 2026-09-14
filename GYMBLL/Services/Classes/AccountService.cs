using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.IdentityViewModels;
using GYMDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GYMBLL.Services.Classes;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

  
    public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
    {
        var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
        var isValidePassword= _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
        return isValidePassword ? user : null;

    }
}
