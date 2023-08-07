using JobFinder.Core.Models.JobListingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobFinder.Common.OtherConsts.PaginationFilter;
namespace JobFinder.Core.Helpers
{
    public class PaginationHelper
    {
        public static IEnumerable<JobListingOutputViewModel> JobListingPaginationFilter(AllJobListingOutputViewModel jobListingOutputViewModels)
        {
            int itemsToSkip = (jobListingOutputViewModels.Page - 1) * ItemsPerPage;
            int itemsLeft = jobListingOutputViewModels.JobLitings.Count() - itemsToSkip;
            int itemsToTake = itemsLeft < ItemsPerPage
                ? itemsLeft
                : ItemsPerPage;

            return jobListingOutputViewModels.JobLitings.Skip(itemsToSkip).Take(itemsToTake);
        }
    }
}
