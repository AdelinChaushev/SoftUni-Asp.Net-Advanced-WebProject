using JobFinder.Core.Models.InterviewViewModel;
using Microsoft.AspNetCore.Mvc;
using JobFinder.Core.Contracts;
namespace JobFinder.Areas.Employer.Controllers
{
    public class InterviewController : BaseController
    {
        private readonly IInterviewServiceInterface interviewService;

        public InterviewController(IInterviewServiceInterface interviewService)
        {
            this.interviewService = interviewService;
        }
        [HttpGet]
        public async Task<IActionResult> ScheduleInterview(string userId, Guid jobListingId)
       => View();
        [HttpPost]
        public async Task<IActionResult> ScheduleInterview(InterviewInputViewModel interviewInputViewModel, string userId, Guid jobListingId)
        {
            if (!ModelState.IsValid)
            {
                return View(interviewInputViewModel);
            }
            if (interviewInputViewModel.StartTime >= interviewInputViewModel.EndTime)
            {
                ModelState.AddModelError("", "End time can not be before or at the same time as the start time.");
                return View(interviewInputViewModel);
            }
            if (interviewInputViewModel.StartTime <= DateTime.Now)
            {
                ModelState.AddModelError("", "The interview start must be in the future.");
                return View(interviewInputViewModel);
            }
            try
            {
                await interviewService.ScheduleInterview(interviewInputViewModel, jobListingId, userId, GetUserId());
            }
            catch (Exception)
            {

                return RedirectToAction("CompanyInterviews","Company");
            }

            return RedirectToAction("CompanyInterviews", "Company");

        }
    }
}
