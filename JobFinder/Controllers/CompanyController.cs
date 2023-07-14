using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }

        public async Task<IActionResult> CompanyInformation(Guid id)
        {
            
            return View();
        }
        [HttpGet]
        public IActionResult Create()
       => View();

        [HttpPost]
        public async Task<IActionResult> Create(CompanyInputViewModel compnay)
        {
            if (!ModelState.IsValid)
            {
                return View(compnay);
            }
            await companyService.AddAsync(ToDbModel(compnay), GetUserId());
            return RedirectToAction("CreateEmployerAccount", "Account");

        }
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
        => new()
        {
            CompanyDescription = compnayViewModel.CompanyDescription,
            CompanyName = compnayViewModel.CompanyName,
        };
    }
}
