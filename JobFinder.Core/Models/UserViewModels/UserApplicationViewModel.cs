namespace JobFinder.Core.Models.UserViewModels
{
    public class UserApplicationViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public Guid? ResumeId { get; set; }
    }
}
