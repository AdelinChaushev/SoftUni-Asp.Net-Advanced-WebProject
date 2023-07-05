using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Model
{
    public class JobListing
    {
        public Guid Id { get; set; }

        [MinLength(3)]
        [MaxLength(35)]
        public string JobTitle { get; set; } = null!;
        [MinLength(30)]
        [MaxLength(500)]
        public string Description { get; set; } = null!;
        [Range(0,double.MaxValue)]
        public decimal SalaryPerMonth { get; set; }
        [Range(1,100)]
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
