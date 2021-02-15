using System.Security.Claims;

namespace WebUi.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        }
    }
}