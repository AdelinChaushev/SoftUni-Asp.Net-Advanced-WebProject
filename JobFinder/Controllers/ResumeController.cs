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
        
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid input");
                return RedirectToAction("AccountSettings", "Account");
            }
            if (User.IsInRole("Employer"))                
            {
                ModelState.AddModelError("", "Employers can not upload resumes");
                return RedirectToAction("AccountSettings", "Account");
            }
            byte[] bytes = new byte[file.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();

            }
            try
            {

            await fileService.UploadResumeAsync(bytes, GetUserId());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "You already have a resume.");
                return RedirectToAction("AccountSettings", "Account");
            }

            return RedirectToAction("AccountSettings", "Account");
        }

        public async Task<IActionResult> Download(Guid? id)
        {
            string path;
            if (id == null)
            {
                try
                {
                    path = await fileService.GetResumePathByUserIdAsync(GetUserId());
                    return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "You do not have a resume to download.");
                    return RedirectToAction("AccountSettings", "Account");
                }
                 
            }
            try
            {
                path = await fileService.GetResumePathByIdAsync(id);
                return PhysicalFile(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            catch (Exception)
            {
                return Redirect("/Employer/Company/CompanyInterviews");
            }
           

            //"application/msword"
        }

        public async Task<IActionResult> Delete() 
        {
            try
            {
            await fileService.DeleteResumeAsync(GetUserId());

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "You do not have a resume to delete.");
                return RedirectToAction("AccountSettings", "Account");
            }
             return RedirectToAction("AccountSettings", "Account");
        }
    }
}
