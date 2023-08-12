using JobFinder.Areas.Employer.Controllers;
using JobFinder.Core.Contracts;
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
    public class PictureControllerTest
    {
        private Mock<ClaimsPrincipal> userMock;
        
        protected ControllerContext testControllerContext;
        private string userId = "1";
       
        private PictureController pictureController;
       

        private Mock<IPictureServiceInterface> pictureService;


        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, userId));

            pictureService = new Mock<IPictureServiceInterface>();


            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            pictureController = new PictureController(pictureService.Object)
            {
                ControllerContext = testControllerContext
            };
         
            pictureController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());

        }
        [Test]
        public async Task UploadPicture()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            pictureService.Setup(s => s.UploadPictureAsync(It.IsAny<byte[]>(), It.IsAny<string>()));

            var result = await pictureController.Upload(file);
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Company");
            Assert.That(actionResult.ActionName == "CompanyPictures");

        }
        [Test]
        public async Task UploadPictureWithModelError()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            pictureController.ModelState.AddModelError("", "");

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            pictureService.Setup(s => s.UploadPictureAsync(It.IsAny<byte[]>(), It.IsAny<string>()));

            var result = await pictureController.Upload(file);
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Company");
            Assert.That(actionResult.ActionName == "CompanyPictures");

        }
        [Test]
        public async Task DeletePicture()
        {
            pictureService.Setup(s => s.DeletePictureAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            var result = await pictureController.Delete(Guid.NewGuid());
            var actionResult = result as RedirectToActionResult;
            Assert.IsNotNull(actionResult);
            Assert.That(actionResult.ControllerName == "Company");
            Assert.That(actionResult.ActionName == "CompanyPictures");
        }
    }
}
