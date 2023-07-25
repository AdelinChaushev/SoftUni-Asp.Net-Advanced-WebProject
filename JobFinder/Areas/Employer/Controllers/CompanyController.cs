using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class CompanyController : BaseController
    { 
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }
        public async Task<IActionResult> Edit()
       => View(await companyService.GetCompanyByUserId(GetUserId()));

        public async Task<IActionResult> Edit(CompanyInputViewModel compnayViewModel)
        {
            var company = ToDbModel(compnayViewModel);
            try
            {
                await companyService.EditedAsync(company, GetUserId());
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return RedirectToAction("CompanySettings");
        }

        public async Task<IActionResult> Delete()
        {
            try
            {
                await companyService.DeleteAsync(GetUserId());
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Redirect("/Home/Index");
        }
       public async Task<IActionResult> CompanySettings(Guid id)
       => View(await companyService.GetCompanyById(id));

        //private Task<CompanyOutputViewModel> ToViewModel(Company company)
        //{
        //    CompanyInputViewModel companyViewModel = new CompanyInputViewModel();
        //    companyViewModel.OwnerId = company.OwnerId;
        //    companyViewModel.CompanyDescription = company.CompanyDescription;
        //    companyViewModel.CompanyName = company.CompanyName;
        //    companyViewModel.Id = company.Id;
        //}
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
         => new()
            {
                CompanyDescription = compnayViewModel.CompanyDescription,
                CompanyName = compnayViewModel.CompanyName,
            };
        
    }
}
