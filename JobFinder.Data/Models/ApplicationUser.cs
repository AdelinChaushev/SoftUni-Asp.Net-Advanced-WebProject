using Microsoft.AspNetCore.Identity;


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
