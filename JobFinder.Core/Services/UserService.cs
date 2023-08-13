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
        private readonly IResumeServiceInterface resumeService;

      

        public UserService(JobFinderDbContext context, IResumeServiceInterface resumeService)
        {
            this.context = context;
            this.resumeService = resumeService;
        }

        public async Task DeleteInterviewsAndJoblistings(string userId)
        {
            var jobLisintgs = await context.JobApplications.AsNoTracking()
                .Where(a => a.UserId == userId).ToListAsync();

            var intervewsDb = await context.Interviews
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var company = await context.Companies
                .FirstOrDefaultAsync(c => c.OwnerId == userId);


            await resumeService.DeleteResumeAsync(userId);
            if(company != null)
            context.Companies.Remove(company);

            context.RemoveRange(intervewsDb);
            context.RemoveRange(jobLisintgs);
            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<UserInterviewOutputViewModel>> GetInterviewsAsync(string userId)
        {
          var interviewsToDelete = await context.Interviews
                .Where(c => c.UserId == userId && c.InterviewEnd < DateTime.Now)
                .ToListAsync();
            context.RemoveRange(interviewsToDelete);
            await context.SaveChangesAsync();
            
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
