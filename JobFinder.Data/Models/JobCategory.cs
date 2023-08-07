using System.ComponentModel.DataAnnotations;

using static JobFinder.Common.DataValidationConstants.JobCategory;


namespace JobFinder.Data.Models
{
    public class JobCategory
    {
        public JobCategory()
        {
            JobListings = new List<JobListing>();
        }

        public Guid Id { get; set; }
        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;
    

        public virtual IEnumerable<JobListing> JobListings { get; set; }
    }
}
