using JobFinder.Core.Contracts;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.UserViewModels;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Core.Services
{
    public class UserService : IUserServiceInterface
    {
        private readonly JobFinderDbContext context;

        public UserService(JobFinderDbContext jobFinderDbContext)
        {
            context = jobFinderDbContext;
        }
        public async Task<IEnumerable<UserInterviewOutputViewModel>> GetInterviewsAsync(string userId)
        {
           var intervewsDb = await context.Interviews
                .Where(c => c.UserId == userId)
                .Select(c => new UserInterviewOutputViewModel()
                {
                    UserId = c.UserId,
                    CompanyId = c.CompanyId,
                    CompnayName = c.Company.CompanyName,
                    JobTitle = c.JobTitle,
                    StartTime = c.InterviewStart,
                    EndTime = c.InterviewEnd,
                })
                .ToListAsync();

            return intervewsDb;



        }

        public async Task<IEnumerable<UserOutputViewModel>> SearchForUser(string keyword)
        {
            List<UserOutputViewModel>? companies = await context.Users
             .Select(c => new UserOutputViewModel
             {
                 Id = c.Id,
                 Email = c.Email,
                 UserName = c.UserName
                 
             })             
            .ToListAsync();
            
            if (!string.IsNullOrWhiteSpace(keyword))
            {

                companies = companies
                   .Where(c => c.UserName.ToLower().Contains(keyword.ToLower()))
                   .ToList();
            }
            return companies;
        }

        public async Task<bool> UserHasCompany(string userId)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.OwnerId == userId);
            if(company == null)
            {
                return false;
            }

            return true;
        }
    }
}
