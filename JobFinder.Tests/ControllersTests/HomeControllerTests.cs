using JobFinder.Areas.Administration.Controllers;
using JobFinder.Areas.Employer.Controllers;
using JobFinder.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Tests.ControllersTests
{
    public class HomeControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        
        protected ControllerContext testControllerContext;
        private string userId = "1";
        private HomeController employerHomeController;
        private JobFinder.Controllers.HomeController homeController;
        private AdminHomeController adminController;

        private Mock<ILogger<HomeController>> employerLogger;
        private Mock<ILogger<JobFinder.Controllers.HomeController>> logger;
        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            employerLogger = new Mock<ILogger<HomeController>>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            employerHomeController = new HomeController(employerLogger.Object)
            {
                ControllerContext = testControllerContext
            };

            employerHomeController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
            logger = new Mock<ILogger<JobFinder.Controllers.HomeController>>();
            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            homeController = new JobFinder.Controllers.HomeController(logger.Object)
            {
                ControllerContext = testControllerContext
            };

            homeController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            adminController = new AdminHomeController()
            {
                ControllerContext = testControllerContext
            };

            homeController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }
        [Test]
        public async Task HomeIndexReturnsView()
        {
            var result = homeController.Index();
            var actionReslut = result as RedirectToActionResult;
            Assert.IsNotNull(actionReslut);
            Assert.That(actionReslut.ActionName == "SearchForJobs");
            Assert.That(actionReslut.ControllerName == "JobListing");
        }
        [Test]
        public async Task HomeIndexRedirectsView()
        {
            userMock.Setup(s => s.IsInRole("Employer")).Returns(true);
            var result = homeController.Index();
            var actionReslut = result as RedirectResult;
            Assert.IsNotNull(actionReslut);
            Assert.That(actionReslut.Url == "/Employer/Home/Index");
        }
        [Test]
        public async Task EmployerHomeIndexRedirects()
        {
            userMock.Setup(s => s.IsInRole("Employer")).Returns(true);
            var result = employerHomeController.Index();
            var actionReslut = result as RedirectToActionResult;
            Assert.IsNotNull(actionReslut);
            Assert.That(actionReslut.ActionName == "CompanyJobListings");
            Assert.That(actionReslut.ControllerName == "Company");

        }
        [Test]
        public async Task AdminHomeIndexRedirects()
        {
            userMock.Setup(s => s.IsInRole("Admin")).Returns(true);
            var result = adminController.Index();
            var actionReslut = result as ViewResult;
            Assert.IsNotNull(actionReslut);

        }
    }
}
