using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static JobFinder.Common.DataValidationConstants.Company;

namespace JobFinder.Data.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameMinLenght)]
        [MinLength(NameMaxLenght)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(DescriptionMinLenght)]
        [MaxLength(DescriptionMinLenght)]
        public string CompanyDescription { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public List<JobListing> JobListings { get; set; }

        public List<Interview> Interviews { get; set; }

        

        public List<Picture> Pictures { get; set; }
    }
}
