using System.ComponentModel.DataAnnotations;
using static JobFinder.Common.DataValidationConstants.Schedule;
namespace JobFinder.Data.Models
{
    public class Schedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(NameMinLenght)]
        [MaxLength(NameMaxLenght)]
        public string WorkingSchedule { get; set; }

        public IEnumerable<JobListing> Jobs { get; set; }
    }
}
