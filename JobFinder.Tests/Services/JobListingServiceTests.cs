using JobFinder.Core.Contracts;
using JobFinder.Core.Models.Enums;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Services;
using JobFinder.Data;
using JobFinder.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Tests.Services
{
    public class JobListingServiceTests
    {
        private List<JobListing> jobListings;
        private List<JobCategory> jobCategories;
        private List<Schedule> schedules;
        private IJobListingServiceInterface jobListingService;

        private JobFinderDbContext context;

        private string userId1 = "1";

        private string userId2 = "2";

        private Guid appleId = Guid.NewGuid();

        private Guid jobCategoryId1 = Guid.NewGuid();
        private Guid jobCategoryId2 = Guid.NewGuid();
        private Guid jobCategoryId3 = Guid.NewGuid();
        private Guid jobCategoryId4 = Guid.NewGuid();
        private string jobCategoryName1 = "Architecture";
        private string jobCategoryName2 = "Tech";
        private string jobCategoryName3 = "Farming";
        private string jobCategoryName4 = "Finnance";

        private Guid jobSchedule1 = Guid.NewGuid();
        private Guid jobSchedule2 = Guid.NewGuid();
        private Guid jobSchedule3 = Guid.NewGuid();
        private Guid jobSchedule4 = Guid.NewGuid();

        private string jobScheduleName1 = "9-5";
        private string jobScheduleName2 = "Weekends";
        private string jobScheduleName3 = "4 hours a day";
        private string jobScheduleName4 = "full working week";


        private Guid jobListingGuid1 = Guid.NewGuid();
        private Guid jobListingGuid2 = Guid.NewGuid();
        private Guid jobListingGuid3 = Guid.NewGuid();
        private Guid jobListingGuid4 = Guid.NewGuid();

        private string jobTitle1 = "Architecture";
        private string jobTitle2 = "Programing";
        private string jobTitle3 = "Marketing";
        private string jobTitle4 = "Farming";

        private string jobDescription1 = "Working with the architecture of MAC,Iphone ..... etc";
        private string jobDescription2 = "Working with the software of MAC,Iphone ..... etc";
        private string jobDescription3 = "Working with the design of MAC,Iphone ..... etc";
        private string jobDescription4 = "Working in the farms of apple";




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
            this.jobListings = new List<JobListing>() {
            new (){  Id = jobListingGuid1, Company = company,JobCategoryId = jobCategoryId1, ScheduleId = jobSchedule1, JobTitle = jobTitle1,Description = jobDescription1,SalaryPerMonth = 10000,VaccantionDays = 20},
            new (){  Id = jobListingGuid2, Company = company,JobCategoryId = jobCategoryId2, ScheduleId = jobSchedule2, JobTitle = jobTitle2,Description = jobDescription2,SalaryPerMonth = 12000,VaccantionDays = 20},
           new (){  Id = jobListingGuid3, Company = company,JobCategoryId = jobCategoryId3, ScheduleId = jobSchedule3, JobTitle = jobTitle3,Description = jobDescription3,SalaryPerMonth = 22000,VaccantionDays = 20},
            new (){  Id = jobListingGuid4, Company = company,JobCategoryId = jobCategoryId4, ScheduleId = jobSchedule4, JobTitle = jobTitle4,Description = jobDescription4,SalaryPerMonth = 22000,VaccantionDays = 20},
            };
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
            Resume resume = new()
            {
                Id = Guid.NewGuid(),
                ResumePath = "",
                UserId = userId2
                
            };
          
            jobCategories = new List<JobCategory>()
            {
              new()
               {
                   Id = jobCategoryId1,
                   Name = "Architecture"
               },
               new()
               {
                   Id = jobCategoryId2,
                   Name = "Tech"
               },
                new()
               {
                   Id = jobCategoryId3,
                   Name = "Farming"
               },
               new()
               {
                   Id = jobCategoryId4,
                   Name = "Finnance"
               }
            };
            schedules = new List<Schedule>()
            {
               new()
               {
                   Id = jobSchedule1,
                   WorkingSchedule = "9-5"
               },
               new()
               {
                   Id = jobSchedule2,
                   WorkingSchedule = "Weekends"
               },
                new()
               {
                   Id = jobSchedule3,
                   WorkingSchedule = "4 hours a day"
               },
               new()
               {
                   Id = jobSchedule4,
                   WorkingSchedule = "full working week"
               },
            };




            var options = new DbContextOptionsBuilder<JobFinderDbContext>()
            .UseInMemoryDatabase(databaseName: "JobFinderDbContext") // Use an in-memory DB
            .Options;
            context = new JobFinderDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.AddRange(this.jobListings); // Add data to the DB
            context.Add(user1);
            context.Add(user2);
            context.Add(resume);
            context.AddRange(jobCategories);
            context.AddRange(schedules);
            context.Add(company);
            context.SaveChanges();

            jobListingService = new JobListingService(context);
        }
        [Test]

        public async Task Test_JobListing_FindByIdAsync()
        {
            var result = await jobListingService.FindByIdAsync(jobListingGuid3);
            Assert.IsNotNull(result);
            Assert.That(result.Id == jobListingGuid3);
            Assert.That(result.JobTitle == jobTitle3);
            Assert.That(result.Description == jobDescription3);
        }

        [Test]
        public async Task Test_JobListing_GetJobCategoriesAsync()
        {
            var result = await jobListingService.GetJobCategoriesAsync();
            Assert.That(result.Count() == 4);
            Assert.That(result.Any(c => c.Id == jobCategoryId1));
            Assert.That(result.Any(c => c.Id == jobCategoryId2));
            Assert.That(result.Any(c => c.Id == jobCategoryId3));
            Assert.That(result.Any(c => c.Id == jobCategoryId4));

        }

        [Test]
        public async Task Test_JobListing_GetSchedulesAsync()
        {
            var result = await jobListingService.GetSchedulesAsync();
            Assert.That(result.Count() == 4);
            Assert.That(result.Any(c => c.Id == jobSchedule1));
            Assert.That(result.Any(c => c.Id == jobSchedule2));
            Assert.That(result.Any(c => c.Id == jobSchedule3));
            Assert.That(result.Any(c => c.Id == jobSchedule4));

        }

        

       
        [Test]
        public async Task Test_JobListing_Delete_Positive()
        {
            await jobListingService.DeleteAsync(jobListingGuid1, userId1);
            Assert.That(context.JobListings.Count() == 3);
            Assert.That(!context.JobListings.Any(c => c.Id == jobListingGuid1));
        }
        [Test]
        public void Test_JobListing_Delete_Negative()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>

                await jobListingService.DeleteAsync(jobListingGuid1, userId2)
            );
        }

        [Test]
        public async Task Test_JobListing_CreateAsync_Positive()
        {
            Guid id = Guid.NewGuid();
            JobListing jobLising = new()
            {
                Id = id,
                CompanyId = appleId,
                JobTitle = jobTitle2,
                Description = jobDescription2,
                JobCategoryId = jobCategoryId2,
                ScheduleId = jobSchedule3,
                SalaryPerMonth = 20000,
                VaccantionDays = 20,


            };

            await jobListingService.CreateAsync(jobLising, userId1);
        }

        [Test]
        public void Test_JobListing_CreateAsync_Negative()
        {
            Guid id = Guid.NewGuid();
            JobListing jobLising = new()
            {
                Id = id,
                CompanyId = appleId,
                JobTitle = jobTitle2,
                Description = jobDescription2,
                JobCategoryId = jobCategoryId2,
                ScheduleId = jobSchedule3,
                SalaryPerMonth = 20000,
                VaccantionDays = 20,


            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await jobListingService.CreateAsync(jobLising, userId2));
        }

        [Test]
        public async Task Test_JobListing_EditAsync_Positive()
        {
            Guid id = Guid.NewGuid();
            JobListing jobLising = new()
            {
                Id = id,
                CompanyId = appleId,
                JobTitle = jobTitle2,
                Description = jobDescription2,
                JobCategoryId = jobCategoryId2,
                ScheduleId = jobSchedule3,
                SalaryPerMonth = 20000,
                VaccantionDays = 20,


            };

            await jobListingService.EditAsync(jobListingGuid2, jobLising, userId1);
        }

        [Test]
        public void Test_JobListing_EditAsync_Negative()
        {
            Guid id = Guid.NewGuid();
            JobListing jobLising = new()
            {
                Id = id,
                CompanyId = appleId,
                JobTitle = jobTitle2,
                Description = jobDescription2,
                JobCategoryId = jobCategoryId2,
                ScheduleId = jobSchedule3,
                SalaryPerMonth = 20000,
                VaccantionDays = 20,


            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await jobListingService.EditAsync(jobListingGuid2, jobLising, userId2));
        }

        [Test]
        public async Task Test_JobListing_SearcForJob()
        {
            AllJobListingOutputViewModel allJobListingOutputViewModel1 = new()
            {
                Keyword = "M",
                OrderBy = (OrderBy)1,
                JobListingSort = 0,
                Category = "None",
                Schedule = "None"
            };
            var jobListings1 = await jobListingService.SearchJobListings(allJobListingOutputViewModel1);
            Assert.That(jobListings1.JobLitings.Count() == 3);
            Assert.That(jobListings1.JobLitings.ToList()[0].Id == jobListingGuid3);
            Assert.That(jobListings1.JobLitings.ToList()[1].Id == jobListingGuid4);
            Assert.That(jobListings1.JobLitings.ToList()[2].Id == jobListingGuid2);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);

            AllJobListingOutputViewModel allJobListingOutputViewModel2 = new()
            {

                OrderBy = (OrderBy)1,
                JobListingSort = (JobListingSort)0,
                Category = jobCategoryName1,
                Schedule = jobScheduleName1
            };
            var jobListings2 = await jobListingService.SearchJobListings(allJobListingOutputViewModel2);
            Assert.That(jobListings2.JobLitings.Count() == 1);
            Assert.That(jobListings2.JobLitings.ToList()[0].Id == jobListingGuid1);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);

            AllJobListingOutputViewModel allJobListingOutputViewModel3 = new()
            {

                OrderBy = (OrderBy)0,
                JobListingSort = (JobListingSort)1,
                Category = jobCategoryName2,
                Schedule = jobScheduleName2
            };
            var jobListings3 = await jobListingService.SearchJobListings(allJobListingOutputViewModel3);
            Assert.That(jobListings3.JobLitings.Count() == 1);
            Assert.That(jobListings3.JobLitings.ToList()[0].Id == jobListingGuid2);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);

            AllJobListingOutputViewModel allJobListingOutputViewModel4 = new()
            {

                OrderBy = (OrderBy)1,
                JobListingSort = (JobListingSort)0,
                Category = jobCategoryName3,
                Schedule = jobScheduleName3
            };
            var jobListings4 = await jobListingService.SearchJobListings(allJobListingOutputViewModel4);
            Assert.That(jobListings4.JobLitings.Count() == 1);
            Assert.That(jobListings4.JobLitings.ToList()[0].Id == jobListingGuid3);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);

            AllJobListingOutputViewModel allJobListingOutputViewModel5 = new()
            {

                OrderBy = (OrderBy)0,
                JobListingSort = (JobListingSort)1,
                Category = jobCategoryName4,
                Schedule = jobScheduleName4
            };
            var jobListings5 = await jobListingService.SearchJobListings(allJobListingOutputViewModel5);
            Assert.That(jobListings5.JobLitings.Count() == 1);
            Assert.That(jobListings5.JobLitings.ToList()[0].Id == jobListingGuid4);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);

            AllJobListingOutputViewModel allJobListingOutputViewModel6 = new()
            {
                Keyword = "M",
                OrderBy = (OrderBy)0,
                JobListingSort = (JobListingSort)1,
                Category = "None",
                Schedule = "None"
            };
            var jobListings6 = await jobListingService.SearchJobListings(allJobListingOutputViewModel1);
            Assert.That(jobListings1.JobLitings.Count() == 3);
            Assert.That(jobListings1.JobLitings.ToList()[0].Id == jobListingGuid3);
            Assert.That(jobListings1.JobLitings.ToList()[1].Id == jobListingGuid4);
            Assert.That(jobListings1.JobLitings.ToList()[2].Id == jobListingGuid2);
            Assert.That(allJobListingOutputViewModel1.Page == 1);
            Assert.True(allJobListingOutputViewModel1.MaxPages == 1);
        }

        [Test]
        public async Task Test_JobListing_ApplyForJob_Possitive()
        {
            await jobListingService.ApplyForJob(jobListingGuid2, userId2);
            Assert.True(await context.JobApplications.AnyAsync(c => c.UserId == userId2 && c.JobListingId == jobListingGuid2));
        }

        [Test]
        public async Task Test_JobListing_ApplyForJob_Negative()
        {
            await jobListingService.ApplyForJob(jobListingGuid2, userId2);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await jobListingService.ApplyForJob(jobListingGuid2, userId2));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await jobListingService.ApplyForJob(jobListingGuid2, userId1));
        }
        [Test]
        public async Task Test_JobListing_GetJobApplications()
        {
            await jobListingService.ApplyForJob(jobListingGuid2, userId2);
            var result = await jobListingService.GetJobApplicationsAsync(jobListingGuid2);
            Assert.That(result.Count() == 1);
            Assert.That(result.Any(c => c.UserId == userId2 && c.JobListingId == jobListingGuid2));
        }


    }

}