using GYMBLL.ViewModels.IdentityViewModels;
using GYMDAL.Entities;

namespace GYMBLL.Services.Interfaces;

public interface IAccountService
{
    ApplicationUser? ValidateUser(LoginViewModel loginViewModel);
}
