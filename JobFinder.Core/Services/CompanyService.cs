using JobFinder.Core.Contracs;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
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
            context.Remove(await GetCompanyByUserId(userId));
           await context.SaveChangesAsync();
        }

        public async Task EditAsync( Company editedEntity,string userId)
        {
            if(editedEntity.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }
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
                    UserName = c.User.UserName,
                    Email = c.User.Email,
                    StartTime = c.InterviewStart,
                    EndTime = c.InterviewEnd


                }).ToListAsync();

            return companyOutputViewModels;



        }

       
    }
}
