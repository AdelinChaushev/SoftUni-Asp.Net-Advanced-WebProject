using JobFinder.Controllers;
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.InterviewViewModel;
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
    public class InterviewControllerTest
    {
        private Mock<ClaimsPrincipal> userMock;
        private Mock<ClaimsPrincipal> employerMock;
        protected ControllerContext testControllerContext;
        private string userId = "1";
        private string employerUserId = "2";
        private InterviewController userInterviewController;
        private Areas.Employer.Controllers.InterviewController employerInterviewController;

        private Mock<IInterviewServiceInterface> interviewService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            interviewService = new Mock<IInterviewServiceInterface>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            userInterviewController = new InterviewController(interviewService.Object)
            {
                ControllerContext = testControllerContext
            };

            userInterviewController.TempData = new TempDataDictionary(
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

            employerInterviewController = new Areas.Employer.Controllers.InterviewController(interviewService.Object)
            {
                ControllerContext = testControllerContext
            };

            employerInterviewController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }
        [Test]
        public async Task ScheduleInterviewResturnsView()
        {
            

            var result = await employerInterviewController.ScheduleInterview("1",Guid.NewGuid());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
           
        }
        [Test]
        public async Task ScheduleInterviewRedirects()
        {

            interviewService
            .Setup(s => s.ScheduleInterview(It.IsAny<InterviewInputViewModel>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));
            var result = await employerInterviewController.ScheduleInterview(new InterviewInputViewModel()
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(1),
            }, "1", Guid.NewGuid()); 
            var actionResult = result as RedirectToActionResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Company");
            Assert.That(actionResult.ActionName == "CompanyInterviews");

        }
        [Test]
        public async Task ScheduleInterviewPostReturnsViewWrongDateTime()
        {

            interviewService
            .Setup(s => s.ScheduleInterview(It.IsAny<InterviewInputViewModel>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));
            var result = await employerInterviewController.ScheduleInterview(new InterviewInputViewModel()
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1),
            }, "1", Guid.NewGuid());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model ,Is.TypeOf<InterviewInputViewModel>());

            var result2 = await employerInterviewController.ScheduleInterview(new InterviewInputViewModel()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
            }, "1", Guid.NewGuid());
            var actionResult2 = result2 as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model, Is.TypeOf<InterviewInputViewModel>());


        }

        [Test]
        public async Task ScheduleInterviewPostReturnsBadRequest()
        {

            interviewService
            .Setup(s => s.ScheduleInterview(It.IsAny<InterviewInputViewModel>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
            var result = await employerInterviewController.ScheduleInterview(new InterviewInputViewModel()
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddMinutes(2),
            }, "1", Guid.NewGuid());
            var actionResult = result as BadRequestResult;

            Assert.IsNotNull(actionResult);
            
          
        }
        [Test]
        public async Task DeleteInterviewRedirects()
        {
            interviewService.Setup(s => s.DeleteInterview(It.IsAny<Guid>(), It.IsAny<string>()));

         var result = await  userInterviewController.Delete("1", Guid.NewGuid());
            var actionResult = result as RedirectToActionResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "User");
            Assert.That(actionResult.ActionName == "Interviews");
            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = employerMock.Object }
            };

            userInterviewController = new InterviewController(interviewService.Object)
            {
                ControllerContext = testControllerContext
            };
            var result2 = await userInterviewController.Delete("1", Guid.NewGuid());
            var actionResult2 = result2 as RedirectResult;

            Assert.IsNotNull(actionResult2);
            Assert.That(actionResult2.Url == "/Employer/Company/CompanyInterviews");
            
        }
        [Test]
        public async Task DeleteInterviewReturnsBadRequest()
        {

            interviewService.Setup(s => s.DeleteInterview(It.IsAny<Guid>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await userInterviewController.Delete("1", Guid.NewGuid());
            var actionResult = result as BadRequestResult;

            Assert.IsNotNull(actionResult);

        }
    }
}
