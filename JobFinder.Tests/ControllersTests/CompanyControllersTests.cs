using JobFinder.Areas.Administration.Controllers;
using JobFinder.Controllers;
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.CompanyViewModels;
using JobFinder.Core.Models.InterviewViewModel;
using JobFinder.Core.Models.JobListingViewModels;
using JobFinder.Core.Models.PictureViewModel;
using JobFinder.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    public class CompanyControllersTests
    {
        private Mock<ClaimsPrincipal> userMock;
        private Mock<ClaimsPrincipal> employerMock;
        private Mock<ClaimsPrincipal> adminMock;
        protected ControllerContext testControllerContext;
        private string userId = "1";
        private string employerUserId = "2";
        private string adminUserId = "3";
        private CompanyController userCompanyController;
        private Areas.Employer.Controllers.CompanyController employerCompanyController;
        private AdminCompanyController adminController;

        private Mock<ICompanyServiceInterface> companyService;
        private Mock<UserManager<ApplicationUser>> userManager;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            companyService = new Mock<ICompanyServiceInterface>();
            

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            userCompanyController = new CompanyController(companyService.Object)
            {
                ControllerContext = testControllerContext
            };

           

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

            employerCompanyController = new Areas.Employer.Controllers.CompanyController(companyService.Object)
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

            adminController = new AdminCompanyController(companyService.Object,userManager.Object)
            {
                ControllerContext = testControllerContext
            };

    
        }
        [Test]
        public async Task CompanyCreateReturnView()
        {
            var result = userCompanyController.Create();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
        }
        [Test]
        public async Task CompanyCreatePostReturnView()
        {
            userCompanyController.ModelState.AddModelError("","");
            companyService.Setup(s => s.AddAsync(It.IsAny<Company>(), userId)); 
            var result = await userCompanyController.Create(new CompanyInputViewModel());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model,Is.TypeOf<CompanyInputViewModel>());
        }
        [Test]
        public async Task CompanyCreateRedirects()
        {
            companyService.Setup(s => s.AddAsync(It.IsAny<Company>(), userId));
            var result = await userCompanyController.Create(new CompanyInputViewModel());
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
           
            Assert.That(actionResult.ControllerName == "Account");
            Assert.That(actionResult.ActionName == "CreateEmployerAccount");
        }
        [Test]
        public async Task CompanyInformation()
        {
           
            companyService.Setup(s => s.GetCompanyById(It.IsAny<Guid>()))
                .ReturnsAsync(new Company()) ;

            companyService.Setup(s => s.GetCompanyPictures(It.IsAny<Guid>()))
                .ReturnsAsync(new List<PictureOutputViewModel>());

            var result = await userCompanyController.CompanyInformation(Guid.NewGuid());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model, Is.TypeOf<CompanyOutputViewModel>());
        }
        [Test]
        public async Task SearchForCompany()
        {
            List<Company> collection = new List<Company>();
            companyService.Setup(s => s.SearchForCompanies(It.IsAny<string>()))
                .ReturnsAsync(collection); 
            var result = await userCompanyController.SearchForCompanies("key");
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            //Assert.That(actionResult.Model, Is.TypeOf<List<CompanyOutputViewModel>>());
        }
        [Test]
        public async Task GetCompanyJobListings()
        {
            List<JobListing> collection = new List<JobListing>();
            companyService.Setup(s => s.GetAllByJobListingsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(collection);
            var result = await userCompanyController.CompanyJobListings(Guid.NewGuid());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model, Is.TypeOf<List<JobListingOutputViewModel>>());
        }
        [Test]
        public async Task CompanyEditReturnsView()
        {
            companyService
                .Setup(s => s.GetCompanyByUserId(It.IsAny<string>()))
                .ReturnsAsync(new Company());

            var result = await employerCompanyController.Edit();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.Model, Is.TypeOf<CompanyInputViewModel>());
        }
        [Test]
        public async Task CompanyEditRedirects()
        {

            companyService
               .Setup(s => s.EditAsync(It.IsAny<Company>(), It.IsAny<string>()));
               
            var result = await employerCompanyController.Edit(new CompanyInputViewModel());
            var actionResult = result as RedirectToActionResult;

            Assert.IsNotNull(actionResult);
            
            Assert.That(actionResult.ActionName == "CompanySettings");
            //Assert.That(actionResult.ControllerName == "Company");
        }
        [Test]
        public async Task CompanyEditPostReturnsView()
        {
            employerCompanyController.ModelState.AddModelError("", "");
            companyService
               .Setup(s => s.EditAsync(It.IsAny<Company>(), It.IsAny<string>()));

            var result = await employerCompanyController.Edit(new CompanyInputViewModel());
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Model, Is.TypeOf<CompanyInputViewModel>());
            //Assert.That(actionResult.ControllerName == "Company");
        }
        [Test]

        public async Task DeleteRedirects()
        {
            companyService
              .Setup(s => s.DeleteAsync( It.IsAny<string>()));
            var result = await employerCompanyController.Delete();
            var actionResult = result as RedirectResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Url == "/Account/DeleteEmployerAccount");
        }
        [Test]
        public async Task CompanyInterviewsReturnsView()
        {
            companyService
              .Setup(s => s.GetCompanyInterviewsAsync(It.IsAny<string>()))
              .ReturnsAsync(new List<CompanyInterviewOutputViewModel>());
            var result = await employerCompanyController.CompanyInterviews();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Model, Is.TypeOf<List<CompanyInterviewOutputViewModel>>());

        }
        [Test]
        public async Task CompanySettingsResurnsView()
        {
            companyService
               .Setup(s => s.GetCompanyByUserId(It.IsAny<string>()))
               .ReturnsAsync(new Company());
            var result = await employerCompanyController.CompanySettings();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Model, Is.TypeOf<CompanySettingViewModel>());
        }
        [Test]
        public async Task GetPicturesReturnsView()
        {
            companyService
               .Setup(s => s.GetCompanyPictures(It.IsAny<string>()))
               .ReturnsAsync(new List<PictureOutputViewModel>());
            var result = await employerCompanyController.CompanyPictures();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Model, Is.TypeOf<List<PictureOutputViewModel>>());
        }
        [Test]
        public async Task GetJobListingsReturnsView()
        {
            companyService
               .Setup(s => s.GetAllByJobListingsAsync(It.IsAny<string>()))
               .ReturnsAsync(new List<JobListing>());
            var result = await employerCompanyController.CompanyJobListings();
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.Model, Is.TypeOf<List<JobListingOutputViewModel>>());
        }
        [Test]
        public async Task AdminSearchForCompany()
        {
            List<Company> collection = new List<Company>();
            companyService.Setup(s => s.SearchForCompanies(It.IsAny<string>()))
                .ReturnsAsync(collection);
            var result = await adminController.CompanySearch("key");
            var actionResult = result as ViewResult;

            Assert.IsNotNull(actionResult);

        }

        [Test]

        public async Task AdminDeleteRedirects()
        {
            companyService
              .Setup(s => s.DeleteAsyncById(It.IsAny<Guid>()));
            userManager.Setup(s => s.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
            var result = await adminController.DeleteCompany(Guid.NewGuid(),userId);
            var actionResult = result as RedirectToActionResult;

            Assert.IsNotNull(actionResult);

            Assert.That(actionResult.ActionName == "CompanySearch");
        }
        [Test]
        public async Task AdminDeleteReturnsBadRequest()
        {
            companyService
              .Setup(s => s.DeleteAsyncById(It.IsAny<Guid>()))
              .ThrowsAsync(new InvalidOperationException());
            var result = await adminController.DeleteCompany(Guid.NewGuid(), userId);
            var actionResult = result as BadRequestResult;

            Assert.IsNotNull(actionResult);

        }
    }
}
