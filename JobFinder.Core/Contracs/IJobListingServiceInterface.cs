using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Data.Models;

namespace JobFinder.Core.Contracs
{
    public interface IJobListingServiceInterface
    {

        public Task<JobListing> FindByIdAsync(Guid id);
        public Task<IEnumerable<JobListing>> GetAllAsync();

        public Task CreateAsync(JobListing jobListing,string userId);

        public Task DeleteAsync(Guid id, string userId);

        public Task EditAsync(Guid id, JobListing edited, string userId);

       public Task<IEnumerable<JobListing>> GetAllByCompanyAsync(string userId);

        public Task ApplyForJob(Guid id,string userId);
        public Task<IEnumerable<JobCategory>> GetJobCategoriesAsync();
        public Task<IEnumerable<Schedule>> GetSchedulesAsync();

        public Task<IEnumerable<ApplicationUser>> GetJobApplicationsAsync(Guid id);

        public Task<AllJobListingOutputViewModel> SearchJobListings(AllJobListingOutputViewModel allJobListingOutputViewModel);

    }
}
