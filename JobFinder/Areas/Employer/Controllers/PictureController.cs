using JobFinder.Core.Contracts;
using JobFinder.Core.Models.FileViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace JobFinder.Areas.Employer.Controllers
{
    public class PictureController : EmployerBaseController
    {
        private readonly IPictureServiceInterface pictureService;

        public PictureController(IPictureServiceInterface pictureServiceInterface)
        {
            this.pictureService = pictureServiceInterface;
        }

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid input");
                return RedirectToAction("CompanyPictures", "Company");
            }            
            byte[] bytes = new byte[file.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();

            }
            await pictureService.UploadPictureAsync(bytes, GetUserId());

            return RedirectToAction("CompanyPictures", "Company");
        }
      

        public async Task<IActionResult> Delete(Guid id)
        {
            await pictureService.DeletePictureAsync(id,GetUserId());
            return RedirectToAction("CompanyPictures", "Company");
        }
    }
}
