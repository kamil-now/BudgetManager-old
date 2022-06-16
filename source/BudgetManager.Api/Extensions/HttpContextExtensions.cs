namespace BudgetManager.Api.Extensions;

using System.Security.Claims;

internal static class HttpContextExtensions {
  public static string GetUserId(this HttpContext context) 
    => (context.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}