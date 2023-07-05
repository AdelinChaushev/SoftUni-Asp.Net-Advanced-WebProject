using System.ComponentModel.DataAnnotations;

namespace JobFinder.Data.Model
{
    public class Schedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string WorkingSchedule { get; set; }

        public IEnumerable<JobListing> Jobs { get; set; }
    }
}
