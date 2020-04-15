using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.EventControllerTests
{
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
        private readonly EventController sut;


        private readonly Mock<IHostEnvironment> env;
        private readonly Mock<IImageUploadWrapper> imageUploadWrapper;
        public DeleteShould()
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
            fixture.repositoryWrapper.Setup(x => x.Event.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync(new Event());
            fixture.mapper.Setup(x => x.Map<EventManagementDTO>(It.IsAny<Event>())).Returns(new EventManagementDTO()).Verifiable();
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");
        }

        [Theory]
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Event.GetEventByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<EventManagementDTO>(vr.Model);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-12)]
        public async void RedirectToIndexIfInvalidValuePassed(int? id)
        {
            var result = await sut.Delete(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Event.GetEventByIdAsync(id), Times.Never);

        }
        [Fact]
        public async void CallDeleteEventModelWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Event.DeleteEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteEvent()
        {
            fixture.repositoryWrapper.Setup(x => x.Event.DeleteEvent(It.IsAny<Event>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Event.DeleteEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
