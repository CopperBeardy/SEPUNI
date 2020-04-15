using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.BuyerControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly BuyerController sut;

        public CreateShould()
        {

            fixture = new SetupFixture();

            sut = new BuyerController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Buyer.GetBuyerByIdAsync(It.IsAny<int>())).ReturnsAsync(new Buyer() { Id = 1 });

            fixture.mapper.Setup(x => x.Map<BuyerManagementDTO>(It.IsAny<Buyer>())).
                    Returns(new BuyerManagementDTO() { Id = 1 });

        }

        [Fact]
        public void ReturnCreateView()
        {
            var result = sut.Create();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Create", vr.ViewName);
        }


        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid BuyerManagementDTO");

           
            var result = await sut.Create(new BuyerManagementDTO() { Id=1});

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Buyer.CreateBuyer(It.IsAny<Buyer>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<BuyerManagementDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateEventInRepository()
        {


           

            fixture.repositoryWrapper.Setup(x => x.Buyer.CreateBuyer(It.IsAny<Buyer>()));

            var result = await sut.Create(It.IsAny<BuyerManagementDTO>());

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Buyer.CreateBuyer(It.IsAny<Buyer>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);

        }


    }
}
