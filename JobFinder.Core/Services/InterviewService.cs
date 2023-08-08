using JobFinder.Core.Contracts;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace JobFinder.Core.Services
{
    public class InterviewService : IInterviewServiceInterface
    {
        private readonly JobFinderDbContext context;

        public InterviewService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }

        public async Task DeleteInterview(Guid companyId, string userId)
        {
            var interview = await context.Interviews.FirstOrDefaultAsync(c => c.UserId == userId && companyId == c.CompanyId);
            if(interview == null)
            {
                throw new InvalidOperationException();
            }
            context.Remove(interview);
            await context.SaveChangesAsync();
        }

        public async Task ScheduleInterview(InterviewInputViewModel interviewInputViewModel, Guid jobId, string userId, string companyOwnerId)
        {

            var job = await context.JobListings.FirstOrDefaultAsync(c => c.Id == jobId);
            var company = await context.Companies.FirstOrDefaultAsync(c => c.JobListings.Any(c => c.Id == jobId));
            if (company.OwnerId != companyOwnerId 
                || userId == companyOwnerId 
                ||await  context.Interviews.AnyAsync(c => c.UserId == userId && c.CompanyId == company.Id))
            {
                throw new InvalidOperationException();
            }
            Interview interview = new()
            {
                CompanyId = company.Id,
                UserId = userId,
                JobTitle = job.JobTitle,
                InterviewStart = interviewInputViewModel.StartTime,
                InterviewEnd = interviewInputViewModel.EndTime,
            };

            await context.AddAsync(interview);
            await context.SaveChangesAsync();

        }

    }
}
