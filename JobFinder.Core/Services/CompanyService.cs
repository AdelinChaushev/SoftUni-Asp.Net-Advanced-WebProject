using JobFinder.Core.Contracs;
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

        public async Task DeleteAsync(Guid id,string userId)
        {
            var company = await GetCompanyById(id);
            if(company.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }
            context.Remove(await GetCompanyById(id));
           await context.SaveChangesAsync();
        }

        public async Task EditedAsync(Guid id, Company editedEntity,string userId)
        {
            if(editedEntity.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }
            Company company = await GetCompanyById(id);
            company.CompanyDescription = editedEntity.CompanyDescription;
            company.CompanyName = editedEntity.CompanyName;

            await context.SaveChangesAsync();

        }

        public async Task<Company> GetCompanyById(Guid id)
         => await context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Company> GetCompanyByuserId(string userId)
        => await context.Companies.FirstOrDefaultAsync(c => c.OwnerId == userId);
    }
}
