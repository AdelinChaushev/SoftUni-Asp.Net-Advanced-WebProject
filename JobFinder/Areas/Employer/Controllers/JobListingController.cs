using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Contracts;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models.JobApplicationViewModels;

namespace JobFinder.Areas.Employer.Controllers
{
    [Authorize(Roles = "Employer")]
    [Area("Employer")]
    public class JobListingController : EmployerBaseController
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

            return RedirectToAction("CompanyJobListings","Company");
            
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
                Schedule = jobLisitng.ScheduleId,
                JobCategory = jobLisitng.JobCategoryId,
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
        

            return RedirectToAction("CompanyJobListings","Company");

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


            return RedirectToAction("CompanyJobListings","Company");

        }
        
        [HttpGet]
        public async Task<IActionResult> JobListingApplications(Guid id)
        {

            IEnumerable<JobApplicationViewModel> applicationUsers = await jobListingService.GetJobApplicationsAsync(id);
             
            return View(applicationUsers);
        }
        

      
        private  JobListing ToDbModel(JobListingInputViewModel compnayViewModel)
        {
           return new JobListing()
            {
                JobTitle = compnayViewModel.JobTitle,
                Description = compnayViewModel.Description,
                SalaryPerMonth = compnayViewModel.SalaryPerMonth,
                VaccantionDays = compnayViewModel.VaccantionDays,
                JobCategoryId = compnayViewModel.JobCategory,
                ScheduleId = compnayViewModel.Schedule,
            };
        }
         
    }
}
