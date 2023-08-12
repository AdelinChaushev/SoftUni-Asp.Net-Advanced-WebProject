using JobFinder.Areas.Administration.Controllers;
using JobFinder.Areas.Employer.Controllers;
using JobFinder.Controllers;
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.UserViewModels;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Tests.ControllersTests
{
    public class UserControllerTest
    {
        private Mock<ClaimsPrincipal> userMock;
        
        protected ControllerContext testControllerContext;
        private Mock<ClaimsPrincipal> adminMock;
        private string userId = "1";
        private string employerUserId = "2";
        private string adminUserId = "3";
        
        private UserController userController;
        private AdminUserController adminController;

        private Mock<IUserServiceInterface> userService;
        private Mock<UserManager<ApplicationUser>> userManager;



        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            userService = new Mock<IUserServiceInterface>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            userController = new UserController(userService.Object)
            {
                ControllerContext = testControllerContext
            };

           

           

            adminMock = new Mock<ClaimsPrincipal>();

            adminMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, adminUserId));

            adminMock.Setup(s => s.IsInRole("Admin"))
                .Returns(true);

            userManager = new Mock<UserManager<ApplicationUser>>(
             new Mock<IUserStore<ApplicationUser>>().Object,
             new Mock<IOptions<IdentityOptions>>().Object,
             new Mock<IPasswordHasher<ApplicationUser>>().Object,
             new IUserValidator<ApplicationUser>[0],
             new IPasswordValidator<ApplicationUser>[0],
             new Mock<ILookupNormalizer>().Object,
             new Mock<IdentityErrorDescriber>().Object,
             new Mock<IServiceProvider>().Object,
             new Mock<ILogger<UserManager<ApplicationUser>>>().Object);



            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = adminMock.Object }
            };

            adminController = new AdminUserController(userService.Object, userManager.Object)
            {
                ControllerContext = testControllerContext
            };


        }
        [Test]
        public async Task UserInterviewsResturnsView()
        {
            userService.Setup(s => s.GetInterviewsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<UserInterviewOutputViewModel>());

            var result = await userController.Interviews();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model,Is.TypeOf<List<UserInterviewOutputViewModel>>());

        }
        [Test]
        public async Task SearchForUserReturnsView()
        {
            userService.Setup(s => s.SearchForUser(It.IsAny<string>()))
               .ReturnsAsync(new List<UserOutputViewModel>());

            var result = await adminController.SearchForUser("");
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model, Is.TypeOf<List<UserOutputViewModel>> ());
        }
        [Test]
        public async Task DeleteUser()
        {
            userManager.Setup(s => s.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            userManager.Setup(s => s.DeleteAsync(It.IsAny<ApplicationUser>()));
               

            var result = await adminController.DeleteUser(userId);
            var actionResult = result as RedirectToActionResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ActionName == "SearchForUser");
            
        }

    }
}
