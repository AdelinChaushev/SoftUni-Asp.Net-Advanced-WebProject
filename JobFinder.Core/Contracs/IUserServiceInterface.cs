using JobFinder.Data.Models;
namespace JobFinder.Core.Contracs
{
    public interface IUserServiceInterface
    {
        public Task<IEnumerable<Interview>> GetInterviewsAsync(string userId);
     

        

       
    }
}
