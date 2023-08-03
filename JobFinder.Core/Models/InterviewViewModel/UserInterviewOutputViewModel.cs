namespace JobFinder.Core.Models.InterviewViewModel
{
    public class UserInterviewOutputViewModel
    {
        public Guid CompanyId { get; set; }

        public string UserId { get; set; }
        public string CompnayName { get; set; }

        public string JobTitle { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
