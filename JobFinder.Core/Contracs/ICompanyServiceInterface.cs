using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Contracs
{
    public interface ICompanyServiceInterface
    {  
        public  Task<Company> GetCompanyById(Guid id);
        public Task<Company> GetCompanyByUserId(string userId);

        public Task<IEnumerable<CompanyInterviewOutputViewModel>> GetCompanyInterviewsAsync(string userId);
        public Task AddAsync(Company company,string userId);
        public Task DeleteAsync(string userId);
        public Task EditAsync( Company editedEntity, string userId);
    }
}
