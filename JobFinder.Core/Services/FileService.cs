using JobFinder.Core.Contracs;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Core.Services
{
    public class FileService : IFileServiceInterface
    {
        private readonly JobFinderDbContext context;

        public FileService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }
        public async Task DeletePictureAsync(Guid id,string userId)
        {
            Picture picture = await context.Pictures.Include(c => c.Company).FirstOrDefaultAsync(c => c.Id == id);
            if(picture.Company.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }
            File.Delete(picture.PicturePath);
            context.Remove(picture);
            await context.SaveChangesAsync();
            

        }

        public async Task DeleteResumeAsync(string userId)
        {
            Resume resume = await context.Resumes.FirstOrDefaultAsync(c => c.UserId == userId);
            if(resume != null)
            {
                File.Delete(resume.ResumePath);
                context.Remove(resume);
                context.SaveChanges();
            }
        }      

        public async Task UploadPictureAsync(MemoryStream stream,Guid companyId)
        {
            string path = "C:/Users/Adi/Dropbox/Pictures" + $"/{GetCompanyName(companyId)}";
            if (Directory.Exists(path))
            {
             Directory.CreateDirectory(path);

            }

            var id = Guid.NewGuid();
            string filePath = Path.Combine(path, id.ToString());
            using (FileStream fs = File.Create(filePath))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(stream.ToArray());
            }
            Picture picture = new Picture()
            {
                Id = id,
                PicturePath = filePath,
                CompanyId = companyId
            };

            await context.AddAsync(picture);
            await context.SaveChangesAsync();
        }

        public async Task UploadResumeAsync(MemoryStream stream, string userId)
        {
            string path = "C:/Users/Adi/Dropbox/Resumes" + $"/{userId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            var id = Guid.NewGuid();
            string filePath = Path.Combine(path, id.ToString());
            using (FileStream fs = File.Create(filePath))
            {
                
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(stream.ToArray());
            }
            Resume resume = new Resume()
            {
                Id = id,
                ResumePath = filePath,
                UserId = userId
            };

            await context.AddAsync(resume);
            await context.SaveChangesAsync();
        }

        private async Task<string> GetCompanyName(Guid  companyId)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            return company.CompanyName;
        }
    }
}
