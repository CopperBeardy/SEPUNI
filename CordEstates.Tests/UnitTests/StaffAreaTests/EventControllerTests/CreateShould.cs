using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.EventControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly EventController sut;
        private Mock<IHostEnvironment> env;
        private Mock<IImageUploadWrapper> imageUploadWrapper;
        public CreateShould()
        {

            fixture = new SetupFixture();

            env = new Mock<IHostEnvironment>();
            imageUploadWrapper = new Mock<IImageUploadWrapper>();
            env.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            sut = new EventController(fixture.Logger.Object,
                                      fixture.repositoryWrapper.Object,
                                      fixture.mapper.Object,
                                      env.Object,
                                      imageUploadWrapper.Object);
            fixture.mapper.Setup(x => x.Map<EventManagementDTO>(It.IsAny<Event>())).Verifiable();
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");
        }
        [Fact]
        public void ReturnCreateView()
        {
            var result = sut.Create();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Create", vr.ViewName);
        }


        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid UploadEventDTO");

            EventManagementDTO uploadEventDTO = new EventManagementDTO() { Active = true };
            var result = await sut.Create(uploadEventDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Event.CreateEvent(It.IsAny<Event>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<EventManagementDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateEventInRepository()
        {


            EventManagementDTO uploadEventDTO = new EventManagementDTO()
            {
                Active = true,
                Photo = new Photo() { ImageLink = "sommelinkg" },
                File = It.IsAny<IFormFile>()
            };

            fixture.repositoryWrapper.Setup(x => x.Event.CreateEvent(It.IsAny<Event>()));

            var result = await sut.Create(uploadEventDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Event.CreateEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);

        }


    }
}
