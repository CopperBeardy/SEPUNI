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


namespace CordEstates.Tests.UnitTests.StaffAreaTests.BuyerControllerTests
{
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly BuyerController sut;

        public EditShould()
        {

            fixture = new SetupFixture();

            sut = new BuyerController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Buyer.GetBuyerByIdAsync(It.IsAny<int>())).ReturnsAsync(new Buyer() {Id = 1});

            fixture.mapper.Setup(x => x.Map<BuyerManagementDTO>(It.IsAny<Buyer>())).
                    Returns(new BuyerManagementDTO() { Id=1});

        }


        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Buyer.GetBuyerByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<BuyerManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-5434)]
        public async void RedirectToIndexForInvalidId(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Buyer.GetBuyerByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            int id = 2;

            var result = await sut.Edit(id,new BuyerManagementDTO() {Id=3 });
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EventDTO");

            var result = await sut.Edit(1);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Buyer.UpdateBuyer(It.IsAny<Buyer>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<BuyerManagementDTO>(vr.Model);
            Assert.IsAssignableFrom<BuyerManagementDTO>(vr.Model);
        }


        

        [Fact]
        public async void ThrowDbUpdateConcurrentExceptionWhenUpdatingBuyer()
        {
            fixture.repositoryWrapper.Setup(x => x.Buyer.UpdateBuyer(It.IsAny<Buyer>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1,new BuyerManagementDTO() {Id=1 });
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Buyer.UpdateBuyer(It.IsAny<Buyer>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
