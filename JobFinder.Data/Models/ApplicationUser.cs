﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Company? Company { get; set; }

        public List<Interview> Interviews { get; set; }

        

        public List<JobApplication> JobApplications { get; set; }

        public Resume? Resume { get; set; }
    }
}
