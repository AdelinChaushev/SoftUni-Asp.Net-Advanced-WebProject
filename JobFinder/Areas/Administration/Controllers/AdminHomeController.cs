using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Mvc;
using  JobFinder.Data.Models;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
