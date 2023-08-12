using JobFinder.Core.Contracts;
using JobFinder.Core.Models.Enums;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Areas.Administration.Controllers
{
    public class AdminJobListingController : AdminBaseController
    {
        private readonly IJobListingServiceInterface jobListingService;

        public AdminJobListingController(IJobListingServiceInterface jobListingService)
        {
            this.jobListingService = jobListingService;
        }
        [HttpGet]
        public async Task<IActionResult> SearchForJobs(string? keyword, string? category, string? schedule, int jobListingSort, int orderBy, int page)
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
        public async Task<IActionResult> Delete(Guid id)
        {


            try
            {
                await jobListingService.DeleteAsync(id, GetUserId());
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }


            return RedirectToAction(nameof(SearchForJobs));

        }
    }
}
