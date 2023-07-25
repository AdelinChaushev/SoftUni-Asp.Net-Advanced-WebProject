namespace JobFinder.Data.Models
{
   using System.ComponentModel.DataAnnotations.Schema;
    public class Interview
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }

        public string  JobTitle { get; set; }

        public DateTime InterviewStart { get; set; }

        public DateTime InterviewEnd { get; set; }
    }
}
