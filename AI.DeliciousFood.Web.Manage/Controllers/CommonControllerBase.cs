using AI.DeliciousFood.Core.Common.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI.DeliciousFood.Web.Manage.Controllers;

public class CommonControllerBase : Controller
{
    public UserInfo UserInfo => GetUserInfo();
    private UserInfo GetUserInfo()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
            return null;

        Claim userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        Claim userNameClaim = User.FindFirst(ClaimTypes.Name);
        Claim userEmailClaim = User.FindFirst(ClaimTypes.Email);

        return new UserInfo
        {
            UserId = long.Parse(userIdClaim?.Value),
            UserName = userNameClaim?.Value,
            UserEmail = userEmailClaim?.Value
        };
    }
}

