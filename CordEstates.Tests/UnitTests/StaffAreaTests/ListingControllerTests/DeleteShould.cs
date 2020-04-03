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

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ListingControllerTests
{
    public class DeleteShould
    {
        private SetupFixture fixture;
        private ListingController sut;
        private Mock<IHostEnvironment> env;
        private Mock<IImageUploadWrapper> imageUploadWrapper;
        public DeleteShould()
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
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<ListingManagementDTO>(vr.Model);
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
            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(id), Times.Never);

        }
        [Fact]
        public async void CallDeleteEventModelWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Listing.DeleteListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteListing()
        {
            fixture.repositoryWrapper.Setup(x => x.Listing.DeleteListing(It.IsAny<Listing>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Listing.DeleteListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
