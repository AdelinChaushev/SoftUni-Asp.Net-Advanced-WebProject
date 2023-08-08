using JobFinder.Core.Models.JobApplicationViewModels;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Data.Models;

namespace JobFinder.Core.Contracts
{
    public interface IJobListingServiceInterface
    {

        public Task<JobListing> FindByIdAsync(Guid id);
        

        public Task CreateAsync(JobListing jobListing,string userId);

        public Task DeleteAsync(Guid id, string userId);

        public Task EditAsync(Guid id, JobListing edited, string userId);

       

        public Task ApplyForJob(Guid id,string userId);
        public Task<IEnumerable<JobCategory>> GetJobCategoriesAsync();
        public Task<IEnumerable<Schedule>> GetSchedulesAsync();

        public Task<IEnumerable<JobApplicationViewModel>> GetJobApplicationsAsync(Guid id);

        public Task<AllJobListingOutputViewModel> SearchJobListings(AllJobListingOutputViewModel allJobListingOutputViewModel);

    }
}
