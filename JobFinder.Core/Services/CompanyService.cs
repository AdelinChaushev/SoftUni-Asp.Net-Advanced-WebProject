using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobApplicationViewModels;
using JobFinder.Core.Models.PictureViewModel;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Core.Services
{
    public class CompanyService : ICompanyServiceInterface
    {
        private readonly JobFinderDbContext context;

        public CompanyService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }
        public async Task AddAsync(Company company, string userId)
        {
            if (await context.Companies.AnyAsync(c => c.OwnerId == userId))
            {
                return;
            }
            company.OwnerId = userId;
            await context.AddAsync(company);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var company = await GetCompanyByUserId(userId);
            if(company.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }
            var companyApplications = context.JobApplications.Where(c => c.JobListing.CompanyId == company.Id);
            var companyListings = context.JobListings.Where(c => c.CompanyId == company.Id);
            context.RemoveRange(companyApplications);
            context.RemoveRange(companyListings);
            context.Remove(company);
           await context.SaveChangesAsync();
        }

        public async Task EditAsync( Company editedEntity,string userId)
        {
         
            Company company = await GetCompanyByUserId(userId);
            company.CompanyDescription = editedEntity.CompanyDescription;
            company.CompanyName = editedEntity.CompanyName;

            await context.SaveChangesAsync();

        }

        public async Task<Company> GetCompanyById(Guid id)
         => await context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Company> GetCompanyByUserId(string userId)
        => await context.Companies.FirstOrDefaultAsync(c => c.OwnerId == userId);

       
        public async Task<IEnumerable<CompanyInterviewOutputViewModel>> GetCompanyInterviewsAsync(string userId)
        {
            var company = await GetCompanyByUserId(userId);

         
            IEnumerable<CompanyInterviewOutputViewModel> companyOutputViewModels = await context.Interviews
                .Where(c => c.CompanyId == company.Id)
                .Select(c => new CompanyInterviewOutputViewModel()
                {
                    UserId = c.UserId,
                    CompanyId = c.CompanyId,
                    UserName = c.User.UserName,
                    Email = c.User.Email,
                    StartTime = c.InterviewStart,
                    EndTime = c.InterviewEnd,
                    JobTitle = c.JobTitle,
                })
                .OrderBy(c => c.StartTime)
                .ToListAsync();

            return companyOutputViewModels;



        }



        public async Task<IEnumerable<Company>> SearchForCompanies(string keyword)
        {
            List<Company>? companies = await context.Companies           
            .ToListAsync();
            if(!string.IsNullOrWhiteSpace(keyword))
            {

             companies =  companies
                .Where(c => c.CompanyName.ToLower().Contains(keyword.ToLower()))
                .ToList();
            }
            return companies;
        }


        public async Task<IEnumerable<JobListing>> GetAllByJobListingsAsync(string userId)
        {
            var company = await GetCompanyByUserId(userId);
            Guid companyId = company.Id;
            return await context.JobListings
                .Include(j => j.JobCategory)
                .Include(j => j.Schedule)
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> GetAllByJobListingsAsync(Guid id)
        {
            var company = await GetCompanyById(id);
            Guid companyId = company.Id;
            return await context.JobListings
                .Include(j => j.JobCategory)
                .Include(j => j.Schedule)
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();
        }

        public  async Task<IEnumerable<PictureOutputViewModel>> GetCompanyPictures(string userId)
        {
            var company = await GetCompanyByUserId(userId);
            return await context.Pictures
                .Where(c => c.CompanyId == company.Id)
                .Select(  c => new PictureOutputViewModel()
                {
                    Id = c.Id,
                    Base64 = $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(c.PicturePath))}",
                   
                })
                .ToListAsync();


        }

        public async Task<IEnumerable<PictureOutputViewModel>> GetCompanyPictures(Guid id)
           => await context.Pictures
                .Where(c => c.CompanyId == id)
               .Select(c => new PictureOutputViewModel()
               {
                   Id = c.Id,
                   Base64 = $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(c.PicturePath))}",
               })
                .ToArrayAsync();

        public async Task DeleteAsyncById(Guid Id)
        {
         var company =   await GetCompanyById(Id);
            if(company == null)
            {
                throw new InvalidOperationException();
            }
            var companyApplications = context.JobApplications.Where(c => c.JobListing.CompanyId == company.Id);
            var companyListings = context.JobListings.Where(c => c.CompanyId == company.Id);
            context.RemoveRange(companyApplications);
            context.RemoveRange(companyListings);
            context.Remove(company);
            await context.SaveChangesAsync();
        }
    }
}