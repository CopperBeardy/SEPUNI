using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.SaleControllerTests
{
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
        private readonly SaleController sut;

        public DeleteShould()
        {

            fixture = new SetupFixture();

            sut = new SaleController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Sale.GetSaleByIdAsync(It.IsAny<int>())).ReturnsAsync(new Sale() { Id = 1 });

            fixture.mapper.Setup(x => x.Map<SaleManagementDTO>(It.IsAny<Sale>())).
                    Returns(new SaleManagementDTO() { Id = 1 });

        }


        [Theory]
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<SaleManagementDTO>(vr.Model);
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
            fixture.repositoryWrapper.Verify(y => y.Sale.GetSaleByIdAsync(id), Times.Never);

        }
        [Fact]
        public async void CallDeleteEventModelWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Sale.DeleteSale(It.IsAny<Sale>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteEvent()
        {
            fixture.repositoryWrapper.Setup(x => x.Sale.DeleteSale(It.IsAny<Sale>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Sale.DeleteSale(It.IsAny<Sale>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
