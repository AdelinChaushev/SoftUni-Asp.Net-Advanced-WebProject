using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Models
{
    public class JobApplication
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(JobListing))]
        public Guid JobListingId { get; set; }

        public JobListing JobListing { get; set; }
    }
}
