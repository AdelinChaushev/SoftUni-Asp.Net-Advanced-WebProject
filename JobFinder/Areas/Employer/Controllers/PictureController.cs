using JobFinder.Core.Contracts;
using JobFinder.Core.Models.FileViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace JobFinder.Areas.Employer.Controllers
{
    public class PictureController : BaseController
    {
        private readonly IPictureServiceInterface pictureService;

        public PictureController(IPictureServiceInterface pictureServiceInterface)
        {
            this.pictureService = pictureServiceInterface;
        }

        public async Task<IActionResult> Upload(FileUploadViewModel file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("AccountSettings", "Account");
            }
            byte[] bytes = new byte[file.File.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                await file.File.CopyToAsync(ms);
                bytes = ms.ToArray();

            }
            await pictureService.UploadPictureAsync(bytes, GetUserId());

            return RedirectToAction("AccountSettings", "Account");
        }
      

        public async Task<IActionResult> Delete(Guid id)
        {
            await pictureService.DeletePictureAsync(id,GetUserId());
            return RedirectToAction("AccountSettings", "Account");
        }
    }
}
