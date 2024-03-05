using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Entities.Identity;


public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public Address? Address { get; set; }
}