using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.SaleControllerTests
{
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly SaleController sut;
        private readonly ClaimsPrincipal claimsPrincipal;
        public EditShould()
        {

            fixture = new SetupFixture();

            sut = new SaleController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);
            fixture.repositoryWrapper
                       .Setup(x => x.Sale.GetSaleByIdAsync(It.IsAny<int>())).ReturnsAsync(new Sale() { Id = 1 });

            fixture.repositoryWrapper
                .Setup(x => x.Listing.GetListingsIdByAddressID(It.IsAny<int>())).Returns(new Listing() { Id = 1 });


            fixture.repositoryWrapper
                .Setup(x => x.Address.GetAllAddressesAsync()).ReturnsAsync(new List<Address>() { new Address() { Id = 1, Number = "23", FirstLine = "test" } });



            fixture.repositoryWrapper
                .Setup(x => x.User.GetUserId(claimsPrincipal))
                .ReturnsAsync("aasfsdfsdfsffsd");

            fixture.mapper.Setup(x => x.Map<SaleManagementDTO>(It.IsAny<Sale>())).
                    Returns(new SaleManagementDTO() { Id=1});

        }

        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<SaleManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-5434)]
        public async void RedirectToIndexForInvalidId(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            int id = 2;

            var result = await sut.Edit(id,new SaleManagementDTO() {Id=3 });
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EventDTO");

            var result = await sut.Edit(1);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Sale.UpdateSale(It.IsAny<Sale>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<SaleManagementDTO>(vr.Model);
            Assert.IsAssignableFrom<SaleManagementDTO>(vr.Model);
        }


        

        [Fact]
        public async void ThrowDbUpdateConcurrentExceptionWhenUpdatingSale()
        {
            fixture.repositoryWrapper.Setup(x => x.Sale.UpdateSale(It.IsAny<Sale>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1,new SaleManagementDTO() {Id=1 });
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Sale.UpdateSale(It.IsAny<Sale>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
