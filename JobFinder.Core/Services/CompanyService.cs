﻿using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobApplicationViewModels;
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
         
            Company company = await GetCompanyByUserId(userId);
            company.CompanyDescription = editedEntity.CompanyDescription;
            company.CompanyName = editedEntity.CompanyName;

            await context.SaveChangesAsync();

        }

        public async Task<Company> GetCompanyById(Guid id)
         => await context.Companies.Include(c => c.JobListings).Include(c => c.Pictures).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Company> GetCompanyByUserId(string userId)
        => await context.Companies.Include(c => c.JobListings).Include(c => c.Pictures).FirstOrDefaultAsync(c => c.OwnerId == userId);

       
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
        => await context.Companies
            .Include(c => c.JobListings)
            .ThenInclude(c =>  c.JobCategory)
            .Include(c => c.JobListings)
            .ThenInclude(c => c.Schedule)
            .Include(c => c.Pictures)
            .Where(c => c.CompanyName.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();


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


    }
}