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
        public Task Add(Company company,string userId);
        public Task Delete(Guid id);
        public Task Edited(Guid id, Company editedEntity, string userId);
    }
}
