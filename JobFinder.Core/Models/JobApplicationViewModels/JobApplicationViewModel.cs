using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Models.JobApplicationViewModels
{
    public class JobApplicationViewModel
    {
        public string UserId { get; set; }

        public Guid? ResumeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Guid JobListingId { get; set; }

    }
}
