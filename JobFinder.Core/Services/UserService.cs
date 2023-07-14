using JobFinder.Core.Contracs;

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
        public async Task<IEnumerable<Interview>> GetInterviewsAsync(string userId)
        {
            return await context.Interviews.Include(c => c.Company).Where(c => c.UserId == userId).ToListAsync();
        }

    }
}
