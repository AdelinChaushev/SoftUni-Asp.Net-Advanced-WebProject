using JobFinder.Core.Contracts;
using JobFinder.Core.Models.FileViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace JobFinder.Controllers
{
    public class ResumeController : BaseController
    {
        private readonly IResumeServiceInterface fileService;

        public ResumeController(IResumeServiceInterface fileService)
        {
            this.fileService = fileService;
        }
        
        public async Task<IActionResult> Upload(FileUploadViewModel file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.IsInRole("Employer") )
            {
                return RedirectToAction("AccountSettings", "Account");
            }
            byte[] bytes = new byte[file.File.Length];
            using (MemoryStream ms = new  MemoryStream())
            {
               await file.File.CopyToAsync(ms);
               bytes = ms.ToArray();

            }
              await fileService.UploadResumeAsync(bytes,GetUserId());
            
            return RedirectToAction("AccountSettings","Account");
        }

        public async Task<IActionResult> Download(Guid? id)
        {
            string path = await fileService.GetResumePathByIdAsync(id);
            return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            //"application/msword"
        }

        public async Task<IActionResult> Delete() 
        {
            await fileService.DeleteResumeAsync(GetUserId());
             return RedirectToAction("AccountSettings", "Account");
        }
    }
}
