using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.BuyerControllerTests
{
    public class DetailShould
    {
  

        private readonly SetupFixture fixture;
        private readonly BuyerController sut;

        public DetailShould()
        {

            fixture = new SetupFixture();

            sut = new BuyerController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Buyer.GetBuyerByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Buyer() { Id = 1 });

            fixture.mapper.Setup(x => x.Map<BuyerManagementDTO>(It.IsAny<Buyer>())).
                    Returns(new BuyerManagementDTO() { Id = 1 });

        }


        [Theory]
        [InlineData(2342)]
        [InlineData(2)]
        [InlineData(4)]
        public async void ReturnValidModelForCorrectId(int id)
        {

            var result = await sut.Details(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Buyer.GetBuyerByIdAsync(id), Times.Once);
            Assert.Equal("Details", vr.ViewName);
            Assert.IsAssignableFrom<BuyerManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(0)]

        [InlineData(-12)]
        public async void RedirectToIndexForInvalidId(int id)
        {
            var result = await sut.Details(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Buyer.GetBuyerByIdAsync(id), Times.Never);

        }



    }
}
