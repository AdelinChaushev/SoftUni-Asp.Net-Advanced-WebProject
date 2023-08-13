using JobFinder.Core.Contracts;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Models.PictureViewModel;

namespace JobFinder.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }
        [HttpGet]
        public async Task<IActionResult> CompanyInformation(Guid id)
        {
            Company companyDbModel = await companyService.GetCompanyById(id);
            var companyViewModel = ToViewModel(companyDbModel);
            companyViewModel.Pictures = await companyService.GetCompanyPictures(id);

            return View(companyViewModel);
        }
        [HttpGet]
        public IActionResult Create()
       => View();

        [HttpGet]
        public async Task<IActionResult> SearchForCompanies(string keyword)
        {
            var company = await companyService.SearchForCompanies(keyword);
            var companyViewModel = ToViewModelMany(company);
            return View(companyViewModel);

        }
        

        [HttpPost]
        public async Task<IActionResult> Create(CompanyInputViewModel compnay)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Invalid input");
                return View(compnay);
            }
            await companyService.AddAsync(ToDbModel(compnay), GetUserId());
            return RedirectToAction("CreateEmployerAccount", "Account");

        }

        [HttpGet]
        public async Task<IActionResult> CompanyJobListings(Guid id)
        {
            IEnumerable<JobListing> jobListings = await companyService.GetAllByJobListingsAsync(id);
            IEnumerable<JobListingOutputViewModel> viewModel = ToViewModelJobListings(jobListings);
            return View(viewModel);
        }


        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
        => new()
        {
            CompanyDescription = compnayViewModel.CompanyDescription,
            CompanyName = compnayViewModel.CompanyName,
        };
        private IEnumerable<CompanyOutputViewModel> ToViewModelMany(IEnumerable<Company> dbModel)
    => dbModel.Select(c => new CompanyOutputViewModel()
    {
        Id = c.Id,
       
        Description = c.CompanyDescription,
        CompanyName = c.CompanyName,
    });
        private CompanyOutputViewModel ToViewModel(Company dbModel)
       => new()
       {
           Id = dbModel?.Id ?? Guid.Empty,
           
           Description = dbModel?.CompanyDescription,
           CompanyName = dbModel?.CompanyName,
       };
        private IEnumerable<JobListingOutputViewModel> ToViewModelJobListings(IEnumerable<JobListing> dbCollection)
        => dbCollection.Select(c => new JobListingOutputViewModel()
        {
            Id = c.Id,
            JobTitle = c.JobTitle,
            Description = c.Description,
            SalaryPerMonth = c.SalaryPerMonth,
            VaccantionDays = c.VaccantionDays,
            CompanyId = c.CompanyId,
            Schedule = c.Schedule.WorkingSchedule,
            JobCategory = c.JobCategory.Name,
        }).ToList();
    }
}
