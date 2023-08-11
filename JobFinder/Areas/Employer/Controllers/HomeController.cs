using JobFinder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobFinder.Areas.Employer.Controllers
{
    
    public class HomeController : EmployerBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            this._logger = _logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("CompanyJobListings", "Company");
        }



       
    }
}