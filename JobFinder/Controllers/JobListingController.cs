using JobFinder.Models.JobListingViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class JobListingController : BaseController
    {
        [HttpGet]
        public IActionResult Add()
        => View();


        [HttpPost]
        public async Task<IActionResult> Add(JobListingInputViewModel jobListingInputViewModel)
        {
            return Ok();
        }
        
    }
}
