
using JobFinder.Data.Models;
using System.ComponentModel.DataAnnotations;
using static JobFinder.Common.DataValidationConstants.JobListing;


namespace JobFinder.Core.Models.JobListingViewModels
{
    public class JobListingInputViewModel
    {
        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string JobTitle { get; set; } = null!;
        [MinLength(DescriptionMinLenght)]
        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; } = null!;
        [Range(MinSalaryPerMonth, MaxSalaryPerMonth)]
        public decimal SalaryPerMonth { get; set; }
        [Range(MinVaccaintionDays, MaxVaccaintionDays)]
        public int VaccantionDays { get; set; }
        public Guid ScheduleId { get; set; }
        public List<Schedule>? Schedules { get; set; }
        public Guid JobCategoryId { get; set; }

        public List<JobCategory>? JobCategories { get; set; }
    }
}
