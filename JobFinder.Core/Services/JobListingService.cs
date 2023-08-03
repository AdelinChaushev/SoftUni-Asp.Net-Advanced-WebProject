using JobFinder.Core.Contracts;
using JobFinder.Core.Models.Enums;
using JobFinder.Core.Models.JobApplicationViewModels;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Data;
using JobFinder.Data.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JobFinder.Core.Services
{
    public class JobListingService : IJobListingServiceInterface
    {
        private readonly JobFinderDbContext context;

        public JobListingService(JobFinderDbContext jobFinderDbContext)
        {
            this.context = jobFinderDbContext;
        }

        public async Task CreateAsync(JobListing jobListing, string userId)
        {
            Guid companyId = await GetCompanyId(userId);
            jobListing.CompanyId = companyId;
            await context.AddAsync(jobListing);
            await context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id,string userId)
        {
            JobListing jobListing = await FindByIdAsync(id);
            Guid companyId = await GetCompanyId(userId);
            if(jobListing.CompanyId != companyId)
            {
                throw new InvalidOperationException();
            }
            context.Remove(jobListing);
            await context.SaveChangesAsync();
        }

        public async Task EditAsync(Guid id,JobListing edited,string userId)
        {
            JobListing jobListing = await FindByIdAsync(id);
            if(jobListing.Company.OwnerId != userId)
            {
                throw new InvalidOperationException();
            }

            jobListing.JobTitle = edited.JobTitle;
            jobListing.Description = edited.Description;
            jobListing.SalaryPerMonth = edited.SalaryPerMonth;
            jobListing.JobCategoryId = edited.JobCategoryId;
            jobListing.ScheduleId = edited.ScheduleId;

          await  context.SaveChangesAsync();
        }

        public async Task<JobListing> FindByIdAsync(Guid id)
        {
            return await context.JobListings.Include(c => c.Company).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<JobListing>> GetAllAsync()
        {
            return await context.JobListings.ToListAsync();
        }
        public async Task<IEnumerable<JobListing>> GetAllByCompanyAsync(string userId)
        {
            Guid companyId = await GetCompanyId(userId);
            return await context.JobListings
                .Include(j => j.JobCategory)
                .Include(j => j.Schedule)
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplicationViewModel>> GetJobApplicationsAsync(Guid id)
          => await context.JobApplications           
            .Select(c => new JobApplicationViewModel()
            {
                UserId = c.UserId,
                Username = c.User.UserName,
                Email = c.User.Email,
                ResumeId = c.User.Resume.Id,
                JobListingId = c.JobListingId,

            })
            
            .ToListAsync();

        public async Task<AllJobListingOutputViewModel> SearchJobListings(AllJobListingOutputViewModel allJobListingOutputViewModel)
        {
            allJobListingOutputViewModel.JobLitings = await context.JobListings.Select(c => new JobListingOutputViewModel()
            {
                Id = c.Id,
                JobTitle = c.JobTitle,
                SalaryPerMonth = c.SalaryPerMonth,
                VaccantionDays = c.VaccantionDays,
                Description = c.Description,
                Schedule = c.Schedule.WorkingSchedule,
                JobCategory = c.JobCategory.Name,
                CompanyId = c.CompanyId,
            }).ToListAsync();
            if(allJobListingOutputViewModel.Keyword != null)
            {
                allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.JobTitle.Contains(allJobListingOutputViewModel.Keyword));
            }
            switch (allJobListingOutputViewModel.Category)
            {
                case "Farming":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.JobCategory == "Farming");
                    break;
                case "Tech":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.JobCategory == "Tech");
                    break;
                case "Architecture":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.JobCategory == "Architecture");
                    break;
                case "Finnance":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.JobCategory == "Finnance");
                    break;              
            }
            switch (allJobListingOutputViewModel.Schedule)
            {
                case "Weekends":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.Schedule == "Weekends");
                    break;
                case "9-5":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.Schedule == "9-5");
                    break;
                case "4 hours a day":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.Schedule == "4 hours a day");
                    break;
                case "full working week":
                    allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.Where(c => c.Schedule == "full working week");
                    break;
            }
            if(allJobListingOutputViewModel.OrderBy == OrderBy.Ascending)
            {
                switch ((int)allJobListingOutputViewModel.JobListingSort)
                {
                    case 0:
                        allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.OrderBy(c => c.SalaryPerMonth);
                        break;
                    case 1:
                        allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.OrderBy(c => c.VaccantionDays);
                        break;
                }
            }
            else
            {
                switch ((int)allJobListingOutputViewModel.JobListingSort)
                {
                    case 0:
                        allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.OrderByDescending(c => c.SalaryPerMonth);
                        break;
                    case 1:
                        allJobListingOutputViewModel.JobLitings = allJobListingOutputViewModel.JobLitings.OrderByDescending(c => c.VaccantionDays);
                        break;
                }
            }

           allJobListingOutputViewModel.MaxPages = allJobListingOutputViewModel.JobLitings.Count() / 10;
            if(allJobListingOutputViewModel.MaxPages == 0)
            {
                allJobListingOutputViewModel.MaxPages = 1;
            }
           else if (allJobListingOutputViewModel.MaxPages % 8 > 0)
            {
                allJobListingOutputViewModel.MaxPages++;
            }

            if (allJobListingOutputViewModel.Page < 1)
            {
                allJobListingOutputViewModel.Page = 1;
            }
            else if (allJobListingOutputViewModel.Page > allJobListingOutputViewModel.MaxPages)
            {
                allJobListingOutputViewModel.MaxPages = allJobListingOutputViewModel.MaxPages;
            }

            allJobListingOutputViewModel.JobLitings = JobListingPaginationFilter(allJobListingOutputViewModel);
            return allJobListingOutputViewModel;
        }
        public async Task<IEnumerable<JobCategory>> GetJobCategoriesAsync()
        => await context.JobCategories.ToListAsync();

        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        => await context.Schedules.ToListAsync();

        private async Task<Guid> GetCompanyId(string userId)
        {
            ApplicationUser applicationUser = await context.Users.Include(c => c.Company).FirstOrDefaultAsync(c => c.Id == userId);

            return applicationUser.Company.Id;
        } 

        public async Task ApplyForJob(Guid id, string userId)
        {
            if(await context.JobApplications.AnyAsync(c => c.UserId == userId && c.JobListingId == id) || 
               await context.Companies.AnyAsync(c => c.OwnerId == userId))
            {
                throw new InvalidOperationException();
            }
            JobApplication jobApplication = new JobApplication()
            {
                UserId = userId,
                JobListingId = id
            };
            await context.AddAsync(jobApplication);
            await context.SaveChangesAsync();
        }



        private  IEnumerable<JobListingOutputViewModel> JobListingPaginationFilter(AllJobListingOutputViewModel jobListingOutputViewModels)
        {
            int itemsToSkip = (jobListingOutputViewModels.Page - 1) * 8;
            int itemsLeft = jobListingOutputViewModels.JobLitings.Count() - itemsToSkip;
            int itemsToTake = itemsLeft < 8
                ? itemsLeft
                : 8;

            return  jobListingOutputViewModels.JobLitings.Skip(itemsToSkip).Take(itemsToTake);
        }

    }
}
