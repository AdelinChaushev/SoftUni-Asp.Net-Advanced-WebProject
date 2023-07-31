using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Models.InterviewViewModel
{
    public class CompanyInterviewOutputViewModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string JobTitle { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
