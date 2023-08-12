using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.UserViewModels;
using JobFinder.Data.Models;
namespace JobFinder.Core.Contracts
{
    public interface IUserServiceInterface
    {
        public Task<IEnumerable<UserInterviewOutputViewModel>> GetInterviewsAsync(string userId);

        public Task<IEnumerable<UserOutputViewModel>> SearchForUser(string keyword);
        public Task<bool> UserHasCompany(string userId);
        public Task DeleteInterviewsAndJoblistings(string userId);
     

        

       
    }
}
