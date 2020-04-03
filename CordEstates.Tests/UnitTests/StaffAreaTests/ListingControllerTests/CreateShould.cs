﻿using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ListingControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private ListingController sut;
        private Mock<IHostEnvironment> env;
        private Mock<IImageUploadWrapper> imageUploadWrapper;
        private ListingManagementDTO listingManagementDTO;
        private Address address;
        public CreateShould()
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
        [Fact]
        public async void ReturnCreateView()
        {
            var result = await sut.Create();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Create", vr.ViewName);
        }


        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid UploadEventDTO");

            var result = await sut.Create(listingManagementDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Listing.CreateListing(It.IsAny<Listing>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<ListingManagementDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateListingInRepository()
        {



            fixture.repositoryWrapper.Setup(x => x.Listing.CreateListing(It.IsAny<Listing>()));

            var result = await sut.Create(listingManagementDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Listing.CreateListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);

        }


    }
}