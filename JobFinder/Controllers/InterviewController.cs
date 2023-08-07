using JobFinder.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class InterviewController : BaseController
    {
        private readonly IInterviewServiceInterface interviewService;

        public InterviewController(IInterviewServiceInterface interviewService)
        {
            this.interviewService = interviewService;
        }
        public async Task<IActionResult> Delete(string userId,Guid companyId)
        {
            try
            {
                await interviewService.DeleteInterview( companyId, userId);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            if (User.IsInRole("Employer"))
            {
                return Redirect("/Employer/Company/CompanyInterviews");
                
            }
            return RedirectToAction("Interviews", "User");
        }

    }
}
