using JobFinder.Core.Contracs;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Core.Services
{
    public class JobListingService : IJobListingServiceInterface
    {
        private readonly JobFinderDbContext context;

        public JobListingService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }

        public async Task CreateAsync(JobListing jobListing, string userId)
        {
            Guid companyId = await GetCompanyId(userId);
            jobListing.CompanyId = companyId;
            await context.AddAsync(jobListing);
            await context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id,string userId)
        {
            JobListing jobListing = await FindByIdAsync(id);
            Guid companyId = await GetCompanyId(userId);
            if(jobListing.CompanyId != companyId)
            {
                throw new InvalidOperationException();
            }
            context.Remove(jobListing);
            await context.SaveChangesAsync();
        }

        public async Task EditAsync(Guid id,JobListing edited,string userId)
        {
            JobListing jobListing = await FindByIdAsync(id);
            if(jobListing.Company.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }

            jobListing.JobTitle = edited.JobTitle;
            jobListing.Description = edited.Description;
            jobListing.SalaryPerMonth = edited.SalaryPerMonth;
            jobListing.JobCategoryId = edited.JobCategoryId;
            jobListing.ScheduleId = edited.ScheduleId;

          await  context.SaveChangesAsync();
        }

        public async Task<JobListing> FindByIdAsync(Guid id)
        {
            return await context.JobListings.Include(c => c.Company).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<JobListing>> GetAllAsync()
        {
            return await context.JobListings.ToListAsync();
        }
        public async Task<IEnumerable<JobListing>> GetAllByCompanyAsync(string userId)
        {
            Guid companyId = await GetCompanyId(userId);
            return await context.JobListings
                .Include(j => j.JobCategory)
                .Include(j => j.Schedule)
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobCategory>> GetJobCategoriesAsync()
        => await context.JobCategories.ToListAsync();

        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        => await context.Schedules.ToListAsync();

        private async Task<Guid> GetCompanyId(string userId)
        {
            ApplicationUser applicationUser = await context.Users.Include(c => c.Company).FirstOrDefaultAsync(c => c.Id == userId);

            return applicationUser.Company.Id;
        } 
    }
}
