using JobFinder.Controllers;
using JobFinder.Core.Contracts;
using JobFinder.Core.Models.FileViewModel;
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
    public class ResumeControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;

        protected ControllerContext testControllerContext;
        private string userId = "1";

        private ResumeController resumeController;


        private Mock<IResumeServiceInterface> resumeService;


        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            resumeService = new Mock<IResumeServiceInterface>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            resumeController = new ResumeController(resumeService.Object)
            {
                ControllerContext = testControllerContext
            };

            resumeController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());

        }
        [Test]
        public async Task UploadPicture()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.docx";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            resumeService.Setup(s => s.UploadResumeAsync(It.IsAny<byte[]>(), It.IsAny<string>()));

            var result = await resumeController.Upload(file);
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Account");
            Assert.That(actionResult.ActionName == "AccountSettings");

        }
        [Test]
        public async Task UploadPictureWithModelError()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.docx";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            resumeController.ModelState.AddModelError("", "");

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            resumeService.Setup(s => s.UploadResumeAsync(It.IsAny<byte[]>(), It.IsAny<string>()));

            var result = await resumeController.Upload(file);
            var actionResult = result as ViewResult;
            Assert.IsNotNull(actionResult);
            
           
            userMock.Setup(c => c.IsInRole("Employer"))
                .Returns(true);

            var result2 = await resumeController.Upload(file);
            var actionResult2 = result2 as ViewResult;
            Assert.IsNotNull(actionResult2);
            //Assert.That(actionResult2.ControllerName == "Account");
            //Assert.That(actionResult2.ActionName == "AccountSettings");

        }
        [Test]
        public async Task DeleteResume()
        {

            resumeService.Setup(s => s.DeleteResumeAsync( It.IsAny<string>()));

            var result = await resumeController.Delete();
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Account");
            Assert.That(actionResult.ActionName == "AccountSettings");
        }
        [Test]
        public async Task DownloadResumeAsync()
        {
            resumeService.Setup(s => s.GetResumePathByIdAsync(It.IsAny<Guid?>()))
                .ReturnsAsync(" ");
            var result = await resumeController.Download(Guid.NewGuid());
            var actionResult = result as PhysicalFileResult;

            Assert.IsNotNull(actionResult);

            resumeService.Setup(s => s.GetResumePathByUserIdAsync(It.IsAny<string>()))
             .ReturnsAsync(" ");
            var result2 = await resumeController.Download(null);
            var actionResult2 = result2 as PhysicalFileResult;

            Assert.IsNotNull(actionResult2);


        }
    }
}
