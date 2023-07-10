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
        public Task<Company> GetCompanyByuserId(string userId);
        public Task AddAsync(Company company,string userId);
        public Task DeleteAsync(Guid id,string userId);
        public Task EditedAsync(Guid id, Company editedEntity, string userId);
    }
}
