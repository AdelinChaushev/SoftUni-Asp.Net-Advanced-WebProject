
using JobFinder.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models.JobListingViewModels
{
    public class JobListingInputViewModel
    {
        [MinLength(3)]
        [MaxLength(35)]
        public string JobTitle { get; set; } = null!;
        [MinLength(30)]
        [MaxLength(500)]
        public string Description { get; set; } = null!;
        [Range(0, double.MaxValue)]
        public decimal SalaryPerMonth { get; set; }
        [Range(1, 100)]
        public int VaccantionDays { get; set; }
        public Guid ScheduleId { get; set; }
        public List<Schedule> Schedules { get; set; }
        public Guid JobCategoryId { get; set; }

        public List<JobCategory> JobCategories { get; set; }
    }
}
