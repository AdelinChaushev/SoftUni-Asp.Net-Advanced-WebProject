using JobFinder.Core.Contracts;
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
    public class PictureServiceTest
    {
        private string userId1 = "1";
        private string userId2 = "2";

        private Guid appleId = Guid.NewGuid();

        private Guid photoId = Guid.NewGuid();
        private string photoPath = "C:/Users/Adi/source/repos/JobFinder/JobFinder.Tests/bin/Debug/net6.0/Path";

        private IPictureServiceInterface userService;
        private JobFinderDbContext context;
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
            Interview interview = new Interview()
            {

                CompanyId = appleId,
                UserId = userId2,
                JobTitle = "Architecture",
                InterviewStart = DateTime.Now.AddDays(1),
                InterviewEnd = DateTime.Now.AddDays(1).AddHours(1),

            };


            var options = new DbContextOptionsBuilder<JobFinderDbContext>()
            .UseInMemoryDatabase(databaseName: "JobFinderDbContext") // Use an in-memory DB
            .Options;
            context = new JobFinderDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add data to the DB
            context.Add(user1);
            context.Add(company);
            context.Add(interview);
            context.SaveChanges();

            userService = new PictureService(context);
        }

        [Test]
        public async Task Test_Picture_Upload()
        {

        }
    }
}
