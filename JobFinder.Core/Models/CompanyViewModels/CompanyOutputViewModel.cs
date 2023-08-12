using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Models.PictureViewModel;

namespace JobFinder.Core.Models.CompanyViewModels
{
    public class CompanyOutputViewModel
    {
        public string OwnerId { get; set; }
        public Guid Id { get; set; }
        public string  CompanyName { get; set; }
        public string  Description { get; set; }

        public IEnumerable<PictureOutputViewModel> Pictures { get; set; }

    }
}
