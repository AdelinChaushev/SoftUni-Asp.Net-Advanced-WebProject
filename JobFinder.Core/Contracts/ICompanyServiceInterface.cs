using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobApplicationViewModels;
using JobFinder.Core.Models.PictureViewModel;
using JobFinder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Contracts
{
    public interface ICompanyServiceInterface
    {  
        public  Task<Company> GetCompanyById(Guid id);
        public Task<Company> GetCompanyByUserId(string userId);

        public Task<IEnumerable<CompanyInterviewOutputViewModel>> GetCompanyInterviewsAsync(string userId);
        public Task AddAsync(Company company,string userId);
        public Task DeleteAsync(string userId);

        public Task DeleteAsyncById(Guid Id);
        public Task EditAsync( Company editedEntity, string userId);

        public Task<IEnumerable<Company>> SearchForCompanies(string keyword);

         

        public Task<IEnumerable<JobListing>> GetAllByJobListingsAsync(string userId);

        public  Task<IEnumerable<JobListing>> GetAllByJobListingsAsync(Guid id);

        public Task<IEnumerable<PictureOutputViewModel>> GetCompanyPictures(string userId);
        public  Task<IEnumerable<PictureOutputViewModel>> GetCompanyPictures(Guid id);



    }
}
