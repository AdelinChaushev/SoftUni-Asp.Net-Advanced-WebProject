using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Models.JobListingViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Employer
{
    public class JobListingController : BaseController
    {
        private readonly IJobListingServiceInterface jobListingService;

        public JobListingController(IJobListingServiceInterface jobListingService)
        {
            this.jobListingService = jobListingService;
        }

        [HttpGet]
        public IActionResult Add()
        => View();


        
        [HttpPost]
        public async Task<IActionResult> Add(JobListingInputViewModel jobListingInputViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(jobListingInputViewModel);
            }
            JobListing jobListing = ToDbModel(jobListingInputViewModel);

            await jobListingService.CreateAsync(jobListing,GetUserId());

            return RedirectToAction("Index","Home");
            
        }


        private JobListing ToDbModel(JobListingInputViewModel compnayViewModel)
         => new JobListing()
         {
             JobTitle = compnayViewModel.JobTitle,
             Description = compnayViewModel.Description,
             SalaryPerMonth = compnayViewModel.SalaryPerMonth,
             VaccantionDays = compnayViewModel.VaccantionDays,
             JobCategoryId = compnayViewModel.JobCategoryId,
             ScheduleId = compnayViewModel.ScheduleId,
         };
    }
}
