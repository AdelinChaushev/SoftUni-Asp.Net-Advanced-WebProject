using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminCompanyController : AdminBaseController
    {
        private readonly ICompanyServiceInterface companyService;

        public AdminCompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> CompanySearch(string keyword)
        {
            var company = await companyService.SearchForCompanies(keyword);
            var outputViewModel = ToViewModelMany(company);
            return View(outputViewModel);
        }

        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            try
            {
               await companyService.DeleteAsyncById(id);
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return RedirectToAction(nameof(CompanySearch));
        }
        private IEnumerable<CompanyOutputViewModel> ToViewModelMany(IEnumerable<Company> dbModel)
        => dbModel.Select(c => new CompanyOutputViewModel()
        {
            Id = c.Id,
            Description = c.CompanyDescription,
            CompanyName = c.CompanyName,
        });
    }
}
