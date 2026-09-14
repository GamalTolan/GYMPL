using Microsoft.AspNetCore.Identity;

namespace GYMDAL.Entities;

public class ApplicationUser :IdentityUser
{
    public  string FristName { get; set; }
    public  string LastName { get; set; }
}
