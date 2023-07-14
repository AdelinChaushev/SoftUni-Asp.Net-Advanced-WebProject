namespace JobFinder.Core.Models.JobListingViewModels
{
    using Enums;
    public class JobListingOutputViewModel
    {
        public Guid Id { get; set; }

        
        public string JobTitle { get; set; }
        
        public string Description { get; set; }
        
        public decimal SalaryPerMonth { get; set; }
     
        public int VaccantionDays { get; set; }
       
        public string JobCategory { get; set; }
        
        public Guid CompanyId { get; set; }

     
        public string Schedule { get; set; }

    }
}
