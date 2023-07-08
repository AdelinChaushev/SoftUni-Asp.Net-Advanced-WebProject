using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Models
{
    public class JobCategory
    {
        public JobCategory()
        {
            JobListings = new List<JobListing>();
        }

        public Guid Id { get; set; }
        [MinLength(3)]
        [MaxLength(35)]
        public string Name { get; set; } = null!;
        [MinLength(20)]
        [MaxLength(270)]
        public string CategoryDescription { get; set; } = null!;

        public virtual IEnumerable<JobListing> JobListings { get; set; }
    }
}
