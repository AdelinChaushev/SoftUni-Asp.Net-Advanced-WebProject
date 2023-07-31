using JobFinder.Core.Contracs;
using JobFinder.Data.Models;
using JobFinder.Core.Models.CompanyViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobListingViewModels;

namespace JobFinder.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class CompanyController : BaseController
    { 
        private readonly ICompanyServiceInterface companyService;

        public CompanyController(ICompanyServiceInterface companyService)
        {
            this.companyService = companyService;
        }
        public async Task<IActionResult> Edit()
       => View(await companyService.GetCompanyByUserId(GetUserId()));

        public async Task<IActionResult> Edit(CompanyInputViewModel compnayViewModel)
        {
            var company = ToDbModel(compnayViewModel);
            try
            {
                await companyService.EditAsync(company, GetUserId());
            }
            catch (Exception)
            {

                return BadRequest();
            }
            return RedirectToAction("CompanySettings");
        }
        [HttpGet]
        public async Task<IActionResult> ScheduleInterview(string userId, Guid jobListingId)
        => View();
        [HttpPost]
        public async Task<IActionResult> ScheduleInterview(InterviewInputViewModel interviewInputViewModel, string userId,Guid  jobListingId)
        {
            if (!ModelState.IsValid)
            {
                return View(interviewInputViewModel);
            }
            if(interviewInputViewModel.StartTime >= interviewInputViewModel.EndTime)
            {
                ModelState.AddModelError("", "End time can not be before or at the same time as the start start");
                return View(interviewInputViewModel);
            }
            try
            {
                await companyService.ScheduleInterview(interviewInputViewModel, jobListingId, userId,GetUserId());
            }
            catch (Exception)
            {

                return RedirectToAction("CompanyInterviews");
            }

            return RedirectToAction("CompanyInterviews");

        }
        [HttpGet]
        public async Task<IActionResult> CompanyInterviews()
        {
          var model = await companyService.GetCompanyInterviewsAsync(GetUserId());
           return View(model);
        }
        public async Task<IActionResult> Delete()
        {
            try
            {
                await companyService.DeleteAsync(GetUserId());
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Redirect("/Home/Index");
        }
       public async Task<IActionResult> CompanySettings(Guid id)
        {
            Company companyDbModel = await companyService.GetCompanyById(id);
            var companyViewModel = ToViewModel(companyDbModel);
            return View(companyViewModel);
        }

        private CompanyOutputViewModel ToViewModel(Company dbModel)
       => new()
       {
           Id = dbModel.Id,
           JobListings = (List<JobListingOutputViewModel>)ToViewModelJobListings(dbModel.JobListings),
           Pictures = dbModel.Pictures.Select(c => c.PicturePath).ToArray(),
           Description = dbModel.CompanyDescription,
           CompanyName = dbModel.CompanyName,
       };
        private Company ToDbModel(CompanyInputViewModel compnayViewModel)
         => new()
            {
                CompanyDescription = compnayViewModel.CompanyDescription,
                CompanyName = compnayViewModel.CompanyName,
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
