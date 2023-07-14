using JobFinder.Core.Contracs;
using JobFinder.Core.Models.FileViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IFileServiceInterface fileService;

        public ResumeController(IFileServiceInterface fileService)
        {
            this.fileService = fileService;
        }
        
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("AccountSettings", "Account");
            }
            using (MemoryStream ms = new MemoryStream())
            {
               await file.CopyToAsync(ms);
              await fileService.UploadResumeAsync(ms,GetUserId());
            }
            
            return RedirectToAction("AccountSettings","Account");
        }
    }
}
