using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Data.Models;
namespace JobFinder.Core.Contracs
{
    public interface IUserServiceInterface
    {
        public Task<IEnumerable<UserInterviewOutputViewModel>> GetInterviewsAsync(string userId);
     

        

       
    }
}
