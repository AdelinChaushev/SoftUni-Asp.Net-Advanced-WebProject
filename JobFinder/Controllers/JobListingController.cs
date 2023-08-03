
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.Enums;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Services;
using JobFinder.Data.Models;
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
        [HttpGet]
        public async Task<IActionResult> SearchForJobs(string? keyword, string? category,string? schedule,int jobListingSort, int orderBy,int page)
        {
            List<JobCategory> jobCategories = (List<JobCategory>)(await jobListingService.GetJobCategoriesAsync());
            List<Schedule> schedules = (List<Schedule>)(await jobListingService.GetSchedulesAsync());
            jobCategories.Insert(0, new JobCategory()
            {
                Id = Guid.NewGuid(),
                Name = "None"
            });
            schedules.Insert(0, new Schedule()
            {
                Id = Guid.NewGuid(),
                WorkingSchedule = "None"
            });
            AllJobListingOutputViewModel allJobListingOutputViewModel = new AllJobListingOutputViewModel()
            {
                Keyword = keyword,
                Category = category,
                Schedule = schedule,
                JobListingSort = (JobListingSort)jobListingSort,
                OrderBy = (OrderBy)jobListingSort,
                Schedules = schedules,
                Categories = jobCategories,
                Page = page

            };
            allJobListingOutputViewModel = await jobListingService.SearchJobListings(allJobListingOutputViewModel);

            return View(allJobListingOutputViewModel);


        }
        [HttpPost]
        public async Task<IActionResult> SearchForJobs(AllJobListingOutputViewModel viewModel)
        {
            

            return RedirectToAction("SearchForJobs", new
            {
                keyword = viewModel.Keyword,
                category = viewModel.Category,
                schedule = viewModel.Schedule,
                jobListingSort = (int)viewModel.JobListingSort,
                orderBy = (int)viewModel.OrderBy,
                page = viewModel.Page

            });

        }

        [HttpGet]
        public async Task<IActionResult> JobListingFullInformation(Guid id)
        {
            var jobListing = await jobListingService.FindByIdAsync(id);
            return View(jobListing);
        }

        
        public async Task<IActionResult> ApplyForJob(Guid id)
        {
            try
            {

            await jobListingService.ApplyForJob(id,GetUserId());

            }
            catch (InvalidOperationException)
            {
                return BadRequest();

            }
            return RedirectToAction("SearchForJobs");
        }
         

        private IEnumerable<JobListingOutputViewModel> ToDbModel(IEnumerable<JobListing> dbCollection)
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
