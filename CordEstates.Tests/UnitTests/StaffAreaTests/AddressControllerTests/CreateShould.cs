using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AddressControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly AddressController sut;
        AddressDTO addressDTO;

        public CreateShould()
        {
            fixture = new SetupFixture();
            addressDTO = new AddressDTO() { Number = "1", FirstLine = "FirstLine", SecondLine = "SecondLine" };
            sut = new AddressController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);
            fixture.repositoryWrapper.Setup(x => x.Address.Create(It.IsAny<Address>())).Verifiable();
            fixture.mapper.Setup(x => x.Map<AddressDTO>(It.IsAny<Address>())).Returns(new AddressDTO());
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
            AddressDTO addressDTO = new AddressDTO() { Number = "1" };
            sut.ModelState.AddModelError(string.Empty, "Invalid AddressDTO");


            var result = await sut.Create(addressDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Address.CreateAddress(It.IsAny<Address>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<AddressDTO>(vr.Model);
        }


        [Fact]
        public async void PassAValidAddressToTheAddressRepository()
        {



            var result = await sut.Create(addressDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Address.CreateAddress(It.IsAny<Address>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);




        }
    }
}
