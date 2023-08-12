using JobFinder.Areas.Administration.Controllers;
using JobFinder.Controllers;
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.JobApplicationViewModels;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Tests.ControllersTests
{
    public class JobListingCotrollerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        private Mock<ClaimsPrincipal> employerMock;
        protected ControllerContext testControllerContext;
        private Mock<ClaimsPrincipal> adminMock;
        private string userId = "1";
        private string employerUserId = "2";
        private string adminUserId = "3";
        private JobListingController userJobListingController;
        private Areas.Employer.Controllers.JobListingController employerJobListingController;
        private AdminJobListingController adminController;

        private Mock<IJobListingServiceInterface> jobListingService;

       

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            jobListingService = new Mock<IJobListingServiceInterface>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            userJobListingController = new JobListingController(jobListingService.Object)
            {
                ControllerContext = testControllerContext
            };

            userJobListingController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());

            employerMock = new Mock<ClaimsPrincipal>();

            employerMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, employerUserId));

            employerMock.Setup(s => s.IsInRole("Employer"))
                .Returns(true);
            

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = employerMock.Object }
            };

            employerJobListingController = new Areas.Employer.Controllers.JobListingController(jobListingService.Object)
            {
                ControllerContext = testControllerContext
            };

            employerJobListingController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());

            adminMock = new Mock<ClaimsPrincipal>();

            adminMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, adminUserId));

            adminMock.Setup(s => s.IsInRole("Admin"))
                .Returns(true);




            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = adminMock.Object }
            };

            adminController = new AdminJobListingController(jobListingService.Object)
            {
                ControllerContext = testControllerContext
            };

            adminController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }
        [Test]
        public async Task SearchForJobReturnsView()
        {
            jobListingService
           .Setup(s => s.GetSchedulesAsync())
           .ReturnsAsync(new List<Schedule>());

            jobListingService
          .Setup(s => s.GetJobCategoriesAsync())
          .ReturnsAsync(new List<JobCategory>());

            jobListingService
            .Setup(s => s.SearchJobListings(It.IsAny<AllJobListingOutputViewModel>()))
            .ReturnsAsync(new AllJobListingOutputViewModel());

            var result = await userJobListingController.SearchForJobs("","None","None",1,1,1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<AllJobListingOutputViewModel>());
        }
        [Test]
        public async Task SearchForJobReturnsRedirect()
        {
            var result = await userJobListingController.SearchForJobs(new AllJobListingOutputViewModel());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ActionName == "SearchForJobs");
        }
        [Test]
        public async Task ApplyForJobRedirects() 
        {
            jobListingService
           .Setup(s => s.ApplyForJob(It.IsAny<Guid>(), It.IsAny<string>()));

            var result = await userJobListingController.ApplyForJob(Guid.NewGuid());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ActionName == "SearchForJobs");

        }

        [Test]
        public async Task JobFullInformation()
        {
            jobListingService
           .Setup(s => s.FindByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(new JobListing()
           {
               Id = Guid.NewGuid(),
               CompanyId = Guid.NewGuid(),
               JobTitle = "",
               Description = "",
               JobCategoryId = Guid.NewGuid(),
               JobCategory = new JobCategory()
               {
                   Name = ""
               },
               Schedule = new Schedule()
               {
                   WorkingSchedule = ""
               },
               ScheduleId = Guid.NewGuid(),
               SalaryPerMonth = 1,
               VaccantionDays = 1,
           }) ;

            var result = await userJobListingController.JobListingFullInformation(Guid.NewGuid());
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model,Is.TypeOf<JobListingOutputViewModel>());

        }
        [Test]
        public async Task AddReturnsView()
        {
            jobListingService
           .Setup(s => s.GetSchedulesAsync())
           .ReturnsAsync(new List<Schedule>());

            jobListingService
          .Setup(s => s.GetJobCategoriesAsync())
          .ReturnsAsync(new List<JobCategory>());

            var result = await employerJobListingController.Add();
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }
        [Test]
        public async Task AddRedirects()
        {
            jobListingService
           .Setup(s => s.GetSchedulesAsync())
           .ReturnsAsync(new List<Schedule>());

            jobListingService
          .Setup(s => s.GetJobCategoriesAsync())
          .ReturnsAsync(new List<JobCategory>());

            jobListingService.Setup(s => s.CreateAsync(It.IsAny<JobListing>(), It.IsAny<string>()));


            var result = await employerJobListingController.Add(new JobListingInputViewModel());
            var viewResult = result as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName == "Company");
            Assert.That(viewResult.ActionName == "CompanyJobListings");
        }
        [Test]
        public async Task AddReturnsPostView()
        {
            employerJobListingController.ModelState.AddModelError("","");

            var result = await employerJobListingController.Add(new JobListingInputViewModel());
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model , Is.TypeOf<JobListingInputViewModel>());
            
        }
        [Test]
        public async Task EditReturnsView()
        {
            jobListingService
           .Setup(s => s.FindByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(new JobListing()
           {
               Id = Guid.NewGuid(),
               CompanyId = Guid.NewGuid(),
               JobTitle = "",
               Description = "",
               JobCategoryId = Guid.NewGuid(),
               JobCategory = new JobCategory()
               {
                   Name = ""
               },
               Schedule = new Schedule()
               {
                   WorkingSchedule = ""
               },
               ScheduleId = Guid.NewGuid(),
               SalaryPerMonth = 1,
               VaccantionDays = 1,
           });
            jobListingService
         .Setup(s => s.GetSchedulesAsync())
         .ReturnsAsync(new List<Schedule>());

            jobListingService
          .Setup(s => s.GetJobCategoriesAsync())
          .ReturnsAsync(new List<JobCategory>());

            var result = await employerJobListingController.Edit(Guid.NewGuid());
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            
        }
        [Test]
        public async Task EditRedirects()
        {
            jobListingService
            .Setup(s => s.EditAsync(It.IsAny<Guid>(), It.IsAny<JobListing>(), It.IsAny<string>()));

            var result = await employerJobListingController.Edit( new JobListingInputViewModel(),Guid.NewGuid());
            var viewResult = result as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName == "Company");
            Assert.That(viewResult.ActionName == "CompanyJobListings");
        }
        [Test]
        public async Task EditPostRetunrnsView()
        {

            employerJobListingController.ModelState.AddModelError("", "");

            var result = await employerJobListingController.Edit(new JobListingInputViewModel(), Guid.NewGuid());
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<JobListingInputViewModel>());
        }
        [Test]
        public async Task EditReturnsBadRequest()
        {
            
            jobListingService
            .Setup(s => s.EditAsync(It.IsAny<Guid>(), It.IsAny<JobListing>(), It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException());

            var result = await employerJobListingController.Edit(new JobListingInputViewModel(), Guid.NewGuid());
            var viewResult = result as BadRequestResult;
            Assert.IsNotNull(viewResult);
      
        }
        [Test]
        public async Task DeleteRedirect()
        {
            jobListingService
           .Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            var result = await employerJobListingController.Delete(Guid.NewGuid());
            var viewResult = result as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName == "Company");
            Assert.That(viewResult.ActionName == "CompanyJobListings");


        }
        [Test]
        public async Task DeleteReturnsBadRequest()
        {
            jobListingService
           .Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()))
           .ThrowsAsync(new InvalidOperationException());

            var result = await employerJobListingController.Delete(Guid.NewGuid());
            var viewResult = result as BadRequestResult;
            Assert.IsNotNull(viewResult);
            


        }
        [Test]
        public async Task GetJobApplications()
        {
            jobListingService.Setup(s => s.GetJobApplicationsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<JobApplicationViewModel>());

            var result = await employerJobListingController.JobListingApplications(Guid.NewGuid());
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<List<JobApplicationViewModel>>());
           
        }

        [Test]
        public async Task AdminSearchForJobReturnsView()
        {
            jobListingService
           .Setup(s => s.GetSchedulesAsync())
           .ReturnsAsync(new List<Schedule>());

            jobListingService
          .Setup(s => s.GetJobCategoriesAsync())
          .ReturnsAsync(new List<JobCategory>());

            jobListingService
            .Setup(s => s.SearchJobListings(It.IsAny<AllJobListingOutputViewModel>()))
            .ReturnsAsync(new AllJobListingOutputViewModel());

            var result = await adminController.SearchForJobs("", "None", "None", 1, 1, 1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<AllJobListingOutputViewModel>());
        }
        [Test]
        public async Task AdminSearchForJobReturnsRedirect()
        {
            var result = await adminController.SearchForJobs(new AllJobListingOutputViewModel());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ActionName == "SearchForJobs");
        }

        [Test]
        public async Task AdminDeleteRedirect()
        {
            jobListingService
           .Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            var result = await adminController.Delete(Guid.NewGuid());
            var viewResult = result as RedirectToActionResult;
            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ActionName == "SearchForJobs");
            


        }
        [Test]
        public async Task AdminDeleteReturnsBadRequest()
        {
            jobListingService
           .Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<string>()))
           .ThrowsAsync(new InvalidOperationException());

            var result = await adminController.Delete(Guid.NewGuid());
            var viewResult = result as BadRequestResult;
            Assert.IsNotNull(viewResult);



        }
    }
}
