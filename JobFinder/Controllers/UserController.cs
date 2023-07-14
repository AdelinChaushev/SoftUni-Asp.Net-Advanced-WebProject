using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class UserController : BaseController
    {
     
        public async Task<IActionResult> Iterviews()
        {
            return View();
        }


    }
}
