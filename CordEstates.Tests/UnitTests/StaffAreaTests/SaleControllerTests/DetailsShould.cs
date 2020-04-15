using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.SaleControllerTests
{
    public class DetailsShould
    {
  

        private readonly SetupFixture fixture;
        private readonly SaleController sut;

        public DetailsShould()
        {

            fixture = new SetupFixture();

            sut = new SaleController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Sale.GetSaleByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Sale() { Id = 1 });

            fixture.mapper.Setup(x => x.Map<SaleManagementDTO>(It.IsAny<Sale>())).
                    Returns(new SaleManagementDTO() { Id = 1 });

        }


        [Theory]
        [InlineData(2342)]
        [InlineData(2)]
        [InlineData(4)]
        public async void ReturnValidModelForCorrectId(int id)
        {

            var result = await sut.Details(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Once);
            Assert.Equal("Details", vr.ViewName);
            Assert.IsAssignableFrom<SaleManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(0)]

        [InlineData(-12)]
        public async void RedirectToIndexForInvalidId(int id)
        {
            var result = await sut.Details(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Never);

        }



    }
}
