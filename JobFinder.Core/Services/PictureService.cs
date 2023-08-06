using JobFinder.Core.Contracts;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Core.Services
{
    public class PictureService : IPictureServiceInterface
    {
        private readonly JobFinderDbContext context;

        public PictureService(JobFinderDbContext jobFinderDbContext)
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


        public async Task UploadPictureAsync(byte[] stream,string userId)
        {
            Guid companyId = await GetCompanyIdByUserIdAsync(userId);
            string path = "C:/Users/Adi/Dropbox/Pictures" + $"/{await GetCompanyName(companyId)}";
            if (!Directory.Exists(path))
            {
             Directory.CreateDirectory(path);

            }

          
            var id = Guid.NewGuid();
            string filePath = path + $"/{id}";
            using (File.Create(filePath))
            {

            }


            await File.WriteAllBytesAsync(filePath, stream.ToArray());
            Picture picture = new Picture()
            {
                Id = id,
                PicturePath = filePath,
                CompanyId = companyId
            };

            await context.AddAsync(picture);
            await context.SaveChangesAsync();
        }
        private async Task<Guid> GetCompanyIdByUserIdAsync(string userId)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.OwnerId == userId);

            return company.Id;
        }
      

        private async Task<string> GetCompanyName(Guid  companyId)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            return company.CompanyName;
        }
    }
}
