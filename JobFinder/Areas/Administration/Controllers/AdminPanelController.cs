using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminPanelController : BaseController
    {
        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }
    }
}
