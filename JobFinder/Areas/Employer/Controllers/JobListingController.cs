using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models.JobApplicationViewModels;

namespace JobFinder.Areas.Employer.Controllers
{
    [Authorize(Roles = "Employer")]
    [Area("Employer")]
    public class JobListingController : BaseController
    {
        private readonly IJobListingServiceInterface jobListingService;

        public JobListingController(IJobListingServiceInterface jobListingService)
        {
            this.jobListingService = jobListingService;
        }

        [HttpGet]
        public async Task<IActionResult>  Add()
        {
            List<JobCategory> jobCategories = (List<JobCategory>)await jobListingService.GetJobCategoriesAsync();
            List<Schedule> schedules = (List<Schedule>)await jobListingService.GetSchedulesAsync();
            JobListingInputViewModel viewModel = new JobListingInputViewModel();


            viewModel.JobCategories = jobCategories;
            viewModel.Schedules = schedules;
            
            if(!viewModel.Schedules.Any() || !viewModel.JobCategories.Any())
            {
                return View("Error");   
            }
          return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(JobListingInputViewModel jobListingInputViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(jobListingInputViewModel);
            }
            JobListing jobListing = ToDbModel(jobListingInputViewModel);

            await jobListingService.CreateAsync(jobListing,GetUserId());

            return RedirectToAction("CompanyJobListings");
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            JobListing jobLisitng = await jobListingService.FindByIdAsync(id);
            JobListingInputViewModel viewModel = new()
            {
                JobTitle = jobLisitng.JobTitle,
                Description = jobLisitng.Description,
                SalaryPerMonth = jobLisitng.SalaryPerMonth,
                VaccantionDays = jobLisitng.VaccantionDays,
                ScheduleId = jobLisitng.ScheduleId,
                JobCategoryId = jobLisitng.JobCategoryId,
                JobCategories = (List<JobCategory>)await jobListingService.GetJobCategoriesAsync(),
                Schedules = (List<Schedule>)await jobListingService.GetSchedulesAsync(),

            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(JobListingInputViewModel jobListingInputViewModel,Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(jobListingInputViewModel);
            }
            JobListing jobListing = ToDbModel(jobListingInputViewModel);
            try
            {
                await jobListingService.EditAsync(id, jobListing, GetUserId());
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        

            return RedirectToAction("CompanyJobListings");

        }
        [HttpPost]
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


            return RedirectToAction("CompanyJobListings");

        }
        [HttpGet]
        public  async Task<IActionResult> CompanyJobListings()
        {
            IEnumerable<JobListing> jobListings = await jobListingService.GetAllByCompanyAsync(GetUserId());
            IEnumerable<JobListingOutputViewModel> viewModel = ToDbModel(jobListings);
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> JobListingApplications(Guid id)
        {

            IEnumerable<JobApplicationViewModel> applicationUsers = await jobListingService.GetJobApplicationsAsync(id);
            if (applicationUsers.Count() == 0)
            {
                return View();
            }          
            return View(applicationUsers);
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

      
        private  JobListing ToDbModel(JobListingInputViewModel compnayViewModel)
        {
           return new JobListing()
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
}
