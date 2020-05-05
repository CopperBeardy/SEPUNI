using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.EventControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly EventController sut;
        private readonly Mock<IHostEnvironment> env;
        private readonly Mock<IImageUploadWrapper> imageUploadWrapper;
        public IndexShould()
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

            fixture.repositoryWrapper
             .Setup(x => x.Event.GetAllEventsAsync())
             .ReturnsAsync(new List<Event>() { It.IsAny<Event>() });
            fixture.mapper.Setup(x => x.Map<List<EventManagementDTO>>(It.IsAny<List<Event>>())).
                Returns(new List<EventManagementDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index("", 5);
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllEvents()
        {

            var result = await sut.Index("", 5);
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<EventManagementDTO>>(vr.Model);

        }



    }
}
