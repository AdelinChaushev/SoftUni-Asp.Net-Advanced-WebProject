using JobFinder.Core.Contracs;
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

        public async Task UploadPictureAsync(MemoryStream stream,Guid companyId)
        {
            string path = "C:/Users/Adi/Dropbox/Pictures" + $"/{GetCompanyName(companyId)}";
            if (Directory.Exists(path))
            {
             Directory.CreateDirectory(path);

            }

            var id = new Guid();
            using (FileStream fs = File.Create(path + id))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(stream.ToArray());
            }
            Picture picture = new Picture()
            {
                Id = id,
                PicturePath = path + id,
                CompanyId = companyId
            };

            await context.AddAsync(picture);
            await context.SaveChangesAsync();
        }
        private async Task<string> GetCompanyName(Guid  companyId)
        {
            return await context.Companies.Select(c => c.CompanyName).FirstOrDefaultAsync();
        }
    }
}
