using GYMDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GYMDAL.Data.Contexts;

public static class IdentityDataSeeding
{
    public static bool SeedData(RoleManager<IdentityRole> roleManager ,UserManager<ApplicationUser> userManager)
    {
        try
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>()
                {

                    new IdentityRole(){Name = "SuperAdmin"},
                    new IdentityRole(){Name = "Admin"}

                };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role.Name).Result)
                    {
                        roleManager.CreateAsync(role).Wait();
                    }
                }
            }

            if (!userManager.Users.Any())
            {
                var superAdmin = new ApplicationUser
                {
                    FristName = "Gamal",
                    LastName = "Tolan",
                    UserName = "GamalTolan",
                    Email = "Gamal@gmail.com",
                    PhoneNumber = "1223334444"

                };

                userManager.CreateAsync(superAdmin, "P@ssw0rd").Wait();
                userManager.AddToRoleAsync(superAdmin, "SuperAdmin").Wait();
                var admin = new ApplicationUser
                {
                    FristName = "Ali",
                    LastName = "Mohamed",
                    UserName = "AliMohamed",
                    Email = "Ali@gmail.com",
                    PhoneNumber = "0102333333"

                };
                userManager.CreateAsync(admin, "P@ssw0rd").Wait();
                userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Seed faild {ex}");
            return false;
        }

        
    }
}
