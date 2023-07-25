using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Models.JobListingViewModels
{
    using Enums;
    using JobFinder.Data.Models;

    public class AllJobListingOutputViewModel
    {
        public IEnumerable<JobListingOutputViewModel> JobLitings { get; set; }

        public string?  Keyword { get; set; }
        public  JobListingSort JobListingSort { get; set; }
        public OrderBy OrderBy { get; set; }

        public int Page { get; set; }

        public int MaxPages { get; set; }
        public List<Schedule> Schedules { get; set; }

        public string Schedule { get; set; }

        public List<JobCategory> Categories { get; set; }

        public string Category { get; set; }
    }
}
