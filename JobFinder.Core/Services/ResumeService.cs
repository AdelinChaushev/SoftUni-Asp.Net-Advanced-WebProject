using JobFinder.Core.Contracts;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Core.Services
{
    public class ResumeService : IResumeServiceInterface
    {

        private readonly JobFinderDbContext context;

        public ResumeService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }
        public async Task UploadResumeAsync(byte[] bytes, string userId)
        {
            if (await context.Resumes.AnyAsync(c => c.UserId == userId))
            {
                throw new InvalidOperationException();
            }
            string path = "C:/Users/Adi/Dropbox/Resumes" + $"/{userId}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }

            var id = Guid.NewGuid();
            string filePath = path + $"/{id}";
            using (File.Create(filePath))
            {

            }

           await  File.WriteAllBytesAsync(filePath, bytes);

            Resume resume = new Resume()
            {
                Id = id,
                ResumePath = filePath,
                UserId = userId
            };
            await context.AddAsync(resume);
            await context.SaveChangesAsync();
        }

        public async Task DeleteResumeAsync(string userId)
        {
            Resume resume = await context.Resumes.FirstOrDefaultAsync(c => c.UserId == userId);
            if (resume == null)
            {
                throw new InvalidOperationException();
            }
            
                
            
            File.Delete(resume.ResumePath);
            context.Remove(resume);
            await context.SaveChangesAsync();
        }

        public async Task<string> GetResumePathByIdAsync(Guid? id)
        {
           
           var resume = await context.Resumes.FirstOrDefaultAsync(c => c.Id == id);
            if(resume == null)
            {
                throw new InvalidOperationException();
            }
            return resume.ResumePath;
        }

        public async Task<string> GetResumePathByUserIdAsync(string id)
        {
            var resume = await context.Resumes.FirstOrDefaultAsync(c => c.UserId == id);
            if (resume == null)
            {
                throw new InvalidOperationException();
            }
            return resume.ResumePath;
        }
    }
}
