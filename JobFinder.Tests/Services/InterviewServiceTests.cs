
using  JobFinder.Data.Models;
using  JobFinder.Data;
using JobFinder.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using JobFinder.Core.Services;
using JobFinder.Core.Models.InterviewViewModel;

namespace JobFinder.Tests.Services
{
    public class InterviewServiceTest
    {
        
        
        private IInterviewServiceInterface interviewService;
        private  JobFinderDbContext context;
        private List<Interview> interviews;

        private Guid appleId = Guid.NewGuid();
        private string userId1 = "1";
        private string userId2 = "2";
        private string userId3 = "3";
        private Guid jobListingGuid1 = Guid.NewGuid();
        private Guid jobCategoryId1 = Guid.NewGuid();
        private Guid jobSchedule1 = Guid.NewGuid();
        [SetUp]
        public void Setup()
        {
            Company company = new Company()
            {
                Id = appleId,
                CompanyName = "Apple"
            ,
                CompanyDescription = "Apple is one of the largest companies in the word that is know for making high end smartphones,laptops....etc."
            ,
                OwnerId = userId1
            };
            JobListing jobListing = new()
            {
                Id = jobListingGuid1,
                Company = company,
                JobCategoryId = jobCategoryId1,
                ScheduleId = jobSchedule1,
                JobTitle = "Architecture",
                Description = "Working with the architecture of MAC,Iphone ..... etc",
                SalaryPerMonth = 10000,
                VaccantionDays = 20
            }
            
            ;
            ApplicationUser user1 = new()
            {
                Id = userId1,
                Email = "SteveJobs@gamil.com",
                UserName = "Steve Jobs",
                EmailConfirmed = true,
                NormalizedUserName = "Steve Jobs",


            };
            ApplicationUser user2 = new()
            {
                Id = userId2,
                Email = "adelin@gamil.com",
                UserName = "Adelin",
                EmailConfirmed = true,
                NormalizedUserName = "ADELIN",


            };
            ApplicationUser user3 = new()
            {
                Id = userId3,
                Email = "peter@gamil.com",
                UserName = "peter",
                EmailConfirmed = true,
                NormalizedUserName = "PETER",


            };
            JobCategory jobCategory = new JobCategory()
            {
             
               
                   Id = jobCategoryId1,
                   Name = "Architecture"
               
            };
            Schedule schedule = new Schedule()
            {
               
                   Id = jobSchedule1,
                   WorkingSchedule = "9-5"
               
            };
            interviews = new List<Interview>()
            {
                new()
                {
                    CompanyId  = appleId,
                    UserId = userId2,
                    JobTitle = "Architecture",
                    InterviewStart = DateTime.Now.AddDays(1),
                    InterviewEnd = DateTime.Now.AddDays(1).AddHours(1),
                }
            };




            var options = new DbContextOptionsBuilder<JobFinderDbContext>()
            .UseInMemoryDatabase(databaseName: "JobFinderDbContext") // Use an in-memory DB
            .Options;
            context = new JobFinderDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            context.Add(jobListing); // Add data to the DB
            context.Add(user1);
            context.Add(user2);
            context.Add(jobCategory);
            context.Add(schedule);
            context.Add(company);
            context.AddRange(interviews);
            context.SaveChanges();

            interviewService = new InterviewService(context);
        }

        [Test]
        public async Task Test_Interview_ScheduleInterview_Positive()
        {
            InterviewInputViewModel interviewInputViewModel = new() 
            {
                StartTime = DateTime.Now.AddDays(2),
                EndTime = DateTime.Now.AddDays(2).AddHours(1) 
            };

            await interviewService.ScheduleInterview(interviewInputViewModel,jobListingGuid1,userId3,userId1);
            Assert.That(context.Interviews.Count() == 2);
            Assert.That(context.Interviews.Any(c => c.UserId == userId2 && c.CompanyId == appleId));
        }

        [Test]
        public  void Test_Interview_ScheduleInterview_Negative()
        {
            InterviewInputViewModel interviewInputViewModel = new()
            {
                StartTime = DateTime.Now.AddDays(2),
                EndTime = DateTime.Now.AddDays(2).AddHours(1)
            };

            
            Assert.ThrowsAsync<InvalidOperationException>(async () => await interviewService.ScheduleInterview(interviewInputViewModel, jobListingGuid1, userId3, userId2));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await interviewService.ScheduleInterview(interviewInputViewModel, jobListingGuid1, userId1, userId2));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await interviewService.ScheduleInterview(interviewInputViewModel, jobListingGuid1, userId3, userId3));
        }

        [Test]

        public async Task Test_Interview_DeleteInterview_Positive()
        {
            await interviewService.DeleteInterview(appleId, userId2);
            Assert.That(context.Interviews.Count() == 0);
        }

        [Test]

        public void Test_Interview_DeleteInterview_Negative()
        {
            
            Assert.ThrowsAsync<InvalidOperationException>(async () => await interviewService.DeleteInterview(appleId, userId1));
        }
    }

}
