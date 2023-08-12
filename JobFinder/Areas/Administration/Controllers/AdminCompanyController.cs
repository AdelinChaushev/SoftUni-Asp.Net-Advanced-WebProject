using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminCompanyController : AdminBaseController
    {
        private readonly ICompanyServiceInterface companyService;



        private readonly UserManager<ApplicationUser> userManager;

        public AdminCompanyController(ICompanyServiceInterface companyService, UserManager<ApplicationUser> userManager)
        {
            this.companyService = companyService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CompanySearch(string keyword)
        {
            var company = await companyService.SearchForCompanies(keyword);
            var outputViewModel = ToViewModelMany(company);
            return View(outputViewModel);
        }

        public async Task<IActionResult> DeleteCompany(Guid id,string? ownerId)
        {
            try
            {
               await companyService.DeleteAsyncById(id);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            if(ownerId != null)
            await userManager.RemoveFromRoleAsync(await userManager.FindByIdAsync(ownerId),"Employer");  

            return RedirectToAction(nameof(CompanySearch));
        }
        private IEnumerable<CompanyOutputViewModel> ToViewModelMany(IEnumerable<Company> dbModel)
        => dbModel.Select(c => new CompanyOutputViewModel()
        {
            Id = c.Id,
            OwnerId = c.OwnerId,
            Description = c.CompanyDescription,
            CompanyName = c.CompanyName,
        });
    }
}
