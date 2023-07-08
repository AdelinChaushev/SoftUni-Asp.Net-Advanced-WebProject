using JobFinder.Core.Contracs;
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
    }
}
