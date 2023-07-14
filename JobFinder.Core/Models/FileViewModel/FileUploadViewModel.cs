using Microsoft.AspNetCore.Http;

namespace JobFinder.Core.Models.FileViewModel
{
    public class FileUploadViewModel 
    {
        public IFormFile File { get; set; }
    }
}
