
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using static JobFinder.Common.DataValidationConstants.JobListing;

namespace JobFinder.Data.Models
{
    public class JobListing
    {
        public Guid Id { get; set; }

        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string JobTitle { get; set; } = null!;
        [MinLength(DescriptionMinLenght)]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; } = null!;
        [Range(MinSalaryPerMonth,MaxSalaryPerMonth)]
        public decimal SalaryPerMonth { get; set; }
        [Range(MinVaccaintionDays,MaxVaccaintionDays)]
        public int VaccantionDays { get; set; }
        [ForeignKey(nameof(JobCategory))]
        public Guid JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
        [ForeignKey(nameof(Schedule))]
        public Guid ScheduleId { get; set; }

        public Schedule Schedule { get; set; }

        public List<JobApplication> UsersApplications { get; set; }

    }
}
