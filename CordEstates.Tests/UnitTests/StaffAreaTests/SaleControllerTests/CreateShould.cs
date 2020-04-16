using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.SaleControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly SaleController sut;
  
        public CreateShould()
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
                .Setup(x => x.Address.GetAllAddressesAsync()).ReturnsAsync(new List<Address>() { new Address() { Id=1,Number="23",FirstLine="test"} });



            fixture.repositoryWrapper
                .Setup(x => x.User.GetUserId(new ClaimsPrincipal()))
                .ReturnsAsync("a test");

            fixture.mapper
                .Setup(x => x.Map<SaleManagementDTO>(It.IsAny<Sale>()))
                .Returns(new SaleManagementDTO() { Id = 1 });
         

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

            sut.ModelState.AddModelError(string.Empty, "Invalid SaleManagementDTO");

           
            var result = await sut.Create(new SaleManagementDTO() { Id=1});

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Sale.CreateSale(It.IsAny<Sale>()), Times.Never);
           fixture.repositoryWrapper.Verify(x => x.Listing.GetListingsIdByAddressID(It.IsAny<int>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<SaleManagementDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateEventInRepository()
        {

            fixture.repositoryWrapper.Setup(x => x.Sale.CreateSale(It.IsAny<Sale>()));
            SaleManagementDTO saleManagementDTO = new SaleManagementDTO() { Id = 1 ,PropertyId=2};



            var result = await sut.Create(saleManagementDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Sale.CreateSale(It.IsAny<Sale>()), Times.Once);

            fixture.repositoryWrapper.Verify(x => x.Listing.GetListingsIdByAddressID(It.IsAny<int>()), Times.Once);

            fixture.repositoryWrapper.Verify(x => x.Listing.UpdateListing(It.IsAny<Listing>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);

        }


    }
}
