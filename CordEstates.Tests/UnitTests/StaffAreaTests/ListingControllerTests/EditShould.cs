using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ListingControllerTests
{
    public class EditShould
    {
        private SetupFixture fixture;
        private ListingController sut;
        private Mock<IHostEnvironment> env;
        private Mock<IImageUploadWrapper> imageUploadWrapper;
        private ListingManagementDTO listingManagementDTO;
        private Address address;
        public EditShould()
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

            address = new Address()
            { FirstLine = "TestFirstLine", Number = "23", TownCity = "TestCity", Postcode = "Xf343xs", Id = 1 };
            fixture.repositoryWrapper
                    .Setup(x => x.Listing.GetListingByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(It.IsAny<Listing>);
            fixture.repositoryWrapper.Setup(x => x.Address.GetAllAddressesAsync()).ReturnsAsync(new List<Address>() { address });
            fixture.mapper.Setup(x => x.Map<ListingManagementDTO>(It.IsAny<Listing>())).Returns(new ListingManagementDTO() { Address = address });
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");

            listingManagementDTO = new ListingManagementDTO()
            { Id = 1, Address = new Address() { Id = 1 }, File = new Mock<IFormFile>().Object };

        }
        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(It.IsAny<int>()), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<ListingManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-5434)]
        public async void RedirectToIndexForInvalidId(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Listing.GetListingByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            int id = 2;

            var result = await sut.Edit(id, listingManagementDTO);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EventDTO");

            var result = await sut.Edit(1, listingManagementDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Listing.UpdateListing(It.IsAny<Listing>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<ListingManagementDTO>(vr.Model);
            Assert.IsAssignableFrom<ListingManagementDTO>(vr.Model);
        }


        [Fact]
        public async void UpdateModelWhenImageChanged()
        {

            var result = await sut.Edit(1, listingManagementDTO);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Listing.UpdateListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void UpdateModelWhenImageNotChanged()
        {

            var result = await sut.Edit(1, listingManagementDTO);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Listing.UpdateListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void ThrowDbUpdateConncurrentExceptionWhenProblemUploading()
        {
            fixture.repositoryWrapper.Setup(x => x.Listing.UpdateListing(It.IsAny<Listing>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1, listingManagementDTO);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Listing.UpdateListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
