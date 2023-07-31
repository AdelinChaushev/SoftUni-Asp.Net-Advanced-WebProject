using JobFinder.Core.Models.JobListingViewModels;
namespace JobFinder.Core.Models.CompanyViewModels
{
    public class CompanyOutputViewModel
    {
        public Guid Id { get; set; }
        public string  CompanyName { get; set; }
        public string  Description { get; set; }

        public string[] Pictures { get; set; }

        public List<JobListingOutputViewModel> JobListings { get; set; }
    }
}
