
using JobFinder.Core.Contracs;
using JobFinder.Core.Models.Enums;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class JobListingController : BaseController
    {
        private readonly IJobListingServiceInterface jobListingService;

        public JobListingController(IJobListingServiceInterface jobListingService)
        {
            this.jobListingService = jobListingService;
        }

        public async Task<IActionResult> SearchForJobs(string category,string? schedule,int? jobListingSort, int? orderBy,int page)
        {
            AllJobListingOutputViewModel allJobListingOutputViewModel = new AllJobListingOutputViewModel()
            {
                Category = category,
                Schedule = schedule,
                JobListingSort = (JobListingSort)jobListingSort,
                OrderBy = (OrderBy)jobListingSort,
                Page = page

            };
            allJobListingOutputViewModel = await jobListingService.SearchJobListings(allJobListingOutputViewModel);

            return View(allJobListingOutputViewModel);


        }

        
    }
}
