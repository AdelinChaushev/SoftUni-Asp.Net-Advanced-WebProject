using JobFinder.Core.Models;
using JobFinder.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger) => this.logger = _logger;

        public IActionResult Index()
        {
            if (User.IsInRole("Employer"))
            {
                return Redirect("/Employer/Home/Index");
            }
            return RedirectToAction("SearchForJobs", "JobListing");
        }
       

    }
}