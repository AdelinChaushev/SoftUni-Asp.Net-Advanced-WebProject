using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Models.CompanyViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Employer
{
    public class CompanyController : BaseController
     { 
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
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
            companyService.Add(ToDbModel(compnay),GetUserId());
            return RedirectToAction("Index","Home");

        }

        //private Task<CompanyOutnputViewModel> ToViewModel(Company company)
        //{
        //    CompanyInputViewModel companyViewModel = new CompanyInputViewModel();
        //    companyViewModel.OwnerId = company.OwnerId;
        //    companyViewModel.CompanyDescription = company.CompanyDescription;
        //    companyViewModel.CompanyName = company.CompanyName;
        //    companyViewModel.Id = company.Id;
        //}
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
         => new Company()
            {
                CompanyDescription = compnayViewModel.CompanyDescription,
                CompanyName = compnayViewModel.CompanyName,
            };
        
    }
}
