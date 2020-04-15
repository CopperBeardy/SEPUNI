using CordEstates.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.RootControllerTests.ListingControllerTests
{
    public class DetailsShould
    {

        private readonly ListingController sut;
        readonly SetupFixture fixture;


        public DetailsShould()
        {
            fixture = new SetupFixture();
            sut = new ListingController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);


            fixture.repositoryWrapper.Setup(x => x.Listing.GetAllListingsAsync()).ReturnsAsync(new List<Listing>() { It.IsAny<Listing>() });
            fixture.mapper.Setup(x => x.Map<ListingDetailDTO>(It.IsAny<Listing>())).Returns(new ListingDetailDTO());
            fixture.mapper.Setup(x => x.Map<List<ExtendedListingDTO>>(It.IsAny<List<Listing>>())).Returns(new List<ExtendedListingDTO>());
        }


        [Fact]
        public async void ReturnCorrectView()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);

        }

        [Fact]
        public async Task ReturnListOfCurrentListings()
        {


            var result = await sut.Index();

            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
            Assert.IsType<List<ExtendedListingDTO>>(vr.Model);

        }

        [Fact]

        public async void ThrowExceptionIfAllListingsNotFound()
        {
            fixture.repositoryWrapper.Setup(x => x.Listing.GetAllListingsForSaleAsync()).Throws<Exception>();

            await Assert.ThrowsAsync<Exception>(() => sut.Index());
        }



    }
}
