using JobFinder.Core.Contracs;
using JobFinder.Core.Models.InterviewViewModel;
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
                    CompnayName = c.Company.CompanyName,
                    StartTime = c.InterviewStart,
                    EndTime = c.InterviewEnd,
                })
                .ToListAsync();

            return intervewsDb;



        }

        
    }
}
