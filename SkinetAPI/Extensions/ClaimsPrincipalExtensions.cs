using System.Security.Claims;

namespace SkinetAPI.Extensions;


public static class ClaimsPrincipalExtensions
{
    public static string RetrieveEmail(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email);
    }
}