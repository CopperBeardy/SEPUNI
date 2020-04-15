using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ListingControllerTests
{
    public class DetailsShould
    {
        private readonly SetupFixture fixture;
        private readonly ListingController sut;
        private readonly Mock<IHostEnvironment> env;
        private readonly Mock<IImageUploadWrapper> imageUploadWrapper;
        public DetailsShould()
        {

            fixture = new SetupFixture();

            env = new Mock<IHostEnvironment>();
            imageUploadWrapper = new Mock<IImageUploadWrapper>();
            env.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            sut = new ListingController(fixture.Logger.Object,
                                      fixture.repositoryWrapper.Object,
                                      fixture.mapper.Object,
                                      env.Object,
                                      imageUploadWrapper.Object);
            fixture.repositoryWrapper.Setup(x => x.Listing.GetListingByIdAsync(It.IsAny<int>())).ReturnsAsync(new Listing());
            fixture.mapper.Setup(x => x.Map<ListingManagementDTO>(It.IsAny<Listing>())).Returns(new ListingManagementDTO()).Verifiable();
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");
        }

        [Theory]
        [InlineData(2342)]
        [InlineData(2)]
        [InlineData(4)]
        public async void ReturnValidModelForCorrectId(int id)
        {

            var result = await sut.Details(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(id), Times.Once);
            Assert.Equal("Details", vr.ViewName);
            Assert.IsAssignableFrom<ListingManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(0)]

        [InlineData(-12)]
        public async void RedirectToIndexForInvalidId(int id)
        {
            var result = await sut.Details(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(id), Times.Never);

        }



    }
}
