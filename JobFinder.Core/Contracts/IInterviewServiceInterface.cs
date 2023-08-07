using JobFinder.Core.Models.InterviewViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Core.Contracts
{
    public interface IInterviewServiceInterface
    {
        public Task ScheduleInterview(InterviewInputViewModel interviewInputViewModel, Guid jobId, string userId, string companyOwnerId);
        public Task DeleteInterview( Guid companyId, string userId);
    }
}
