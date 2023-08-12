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
    public class ResumeServiceTests
    {
        private string userId1 = "Test1";
        private string userId2 = "Test2";

        private Guid resumeId = Guid.NewGuid();        
        private string resumePath = "C:/Users/Adi/source/repos/JobFinder/JobFinder.Tests/bin/Debug/net6.0/Path";
        private IResumeServiceInterface resumeService;
        private JobFinderDbContext context;

        [SetUp]
        public void Setup()
        {

            Resume resume = new()
            {
                Id = resumeId,
                ResumePath = resumePath,
                UserId = userId2
                
            };


            ApplicationUser user1 = new()
            {
                Id = userId1,
                Email = "SteveJobs@gamil.com",
                UserName = "test",
                EmailConfirmed = true,
                NormalizedUserName = "Steve Jobs",


            };
            ApplicationUser user2 = new()
            {
                Id = userId2,
                Email = "",
                UserName = "",
                EmailConfirmed = true,
                NormalizedUserName = "",


            };




            var options = new DbContextOptionsBuilder<JobFinderDbContext>()
            .UseInMemoryDatabase(databaseName: "JobFinderDbContext") // Use an in-memory DB
            .Options;
            context = new JobFinderDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add data to the DB
            context.Add(user1);
            context.Add(user2);
            context.Add(resume);


            context.SaveChanges();

            resumeService = new ResumeService(context);
        }
        [Test]
        public async Task Test_Resume_Upload()
        {
            await resumeService.UploadResumeAsync( await File.ReadAllBytesAsync("C:\\Users\\Adi\\Dropbox\\Resumes\\Test1\\7504feb7-7eeb-4ddf-bddb-e211092b48ba"),userId1);
            Assert.That(context.Resumes.Count() == 2);
            Assert.That(context.Resumes.Any(c => c.ResumePath.Contains("C:/Users/Adi/Dropbox/Resumes" + $"/{userId1}")));
        }
        [Test]
        public async Task Test_Resume_Delete_Positive()
        {
            await resumeService.UploadResumeAsync(await File.ReadAllBytesAsync("C:\\Users\\Adi\\Dropbox\\Resumes\\Test1\\7504feb7 - 7eeb - 4ddf - bddb - e211092b48ba"), userId1);
            await resumeService.DeleteResumeAsync(userId1);
            Assert.That(context.Resumes.Count() == 0);

        }
        [Test]
        public async Task Test_Resume_GetById()
        {
            var resume = await resumeService.GetResumePathByIdAsync(resumeId);
            Assert.That(resume == resumePath);

        }
        [Test]
        public async Task Test_Resume_GetByUserId()
        {
            var resume = await resumeService.GetResumePathByUserIdAsync(userId2);
            Assert.That(resume == resumePath);

        }
    }
}
