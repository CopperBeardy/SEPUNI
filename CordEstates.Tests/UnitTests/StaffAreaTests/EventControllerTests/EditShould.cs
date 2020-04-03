using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.EventControllerTests
{
    public class EditShould
    {
        private SetupFixture fixture;
        private EventController sut;
        private Mock<IHostEnvironment> env;
        private Mock<IImageUploadWrapper> imageUploadWrapper;
        EventManagementDTO eve;
        public EditShould()
        {

            fixture = new SetupFixture();

            env = new Mock<IHostEnvironment>();

            imageUploadWrapper = new Mock<IImageUploadWrapper>();
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");

            env.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");

            sut = new EventController(fixture.Logger.Object,
                                      fixture.repositoryWrapper.Object,
                                      fixture.mapper.Object,
                                      env.Object,
                                      imageUploadWrapper.Object);

            fixture.repositoryWrapper.Setup(x => x.Event.GetEventByIdAsync(It.IsAny<int>())).ReturnsAsync(new Event());

            fixture.mapper.Setup(x => x.Map<EventManagementDTO>(It.IsAny<Event>())).Returns(new EventManagementDTO()).Verifiable();



            eve = new EventManagementDTO() { Id = 1 };
        }

        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Event.GetEventByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<EventManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-5434)]
        public async void RedirectToIndexForInvalidId(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Event.GetEventByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            int id = 2;

            var result = await sut.Edit(id, eve);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EventDTO");

            var result = await sut.Edit(1, eve);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Event.UpdateEvent(It.IsAny<Event>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<EventManagementDTO>(vr.Model);
            Assert.IsAssignableFrom<EventManagementDTO>(vr.Model);
        }


        [Fact]
        public async void UpdateModelWhenImageChanged()
        {
            eve.File = new Mock<IFormFile>().Object;

            var result = await sut.Edit(1, eve);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Event.UpdateEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void UpdateModelWhenImageNotChanged()
        {

            var result = await sut.Edit(1, eve);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Event.UpdateEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void ThrowDbUpdateConncurrentExceptionWhenProblemUploading()
        {
            fixture.repositoryWrapper.Setup(x => x.Event.UpdateEvent(It.IsAny<Event>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1, eve);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Event.UpdateEvent(It.IsAny<Event>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
