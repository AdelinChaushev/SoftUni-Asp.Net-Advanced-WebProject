using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFinder.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administrations")]
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
