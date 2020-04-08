using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AddressControllerTests
{
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly AddressController sut;
        private readonly Address address;

        public EditShould()
        {

            fixture = new SetupFixture();

            sut = new AddressController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);
            address = new Address() { Id = 1, FirstLine = "FirstLine", SecondLine = "SecondLine" };
            fixture.repositoryWrapper.Setup(x => x.Address.GetAddressByIdAsync(It.IsAny<int>())).ReturnsAsync(address);


            fixture.mapper.Setup(x => x.Map<AddressDTO>(It.IsAny<Address>())).Returns(new AddressDTO());

        }

        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Address.GetAddressByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<AddressDTO>(vr.Model);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-2234)]
        public async void RedirectToIndexIfInvalidValuePassed(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Address.GetAddressByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {
            AddressDTO addressDTO = new AddressDTO() { Number = "1" };
            sut.ModelState.AddModelError(string.Empty, "Invalid AddressDTO");


            var result = await sut.Edit(addressDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Address.UpdateAddress(It.IsAny<Address>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<AddressDTO>(vr.Model);
            Assert.IsAssignableFrom<AddressDTO>(vr.Model);
        }


        [Fact]
        public async void UpdateModelWithAValidModelState()
        {
            AddressDTO addressDTO = new AddressDTO() { Number = "1" };


            var result = await sut.Edit(addressDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Address.UpdateAddress(It.IsAny<Address>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<AddressDTO>(vr.Model);
            Assert.IsAssignableFrom<AddressDTO>(vr.Model);
        }

    }
}
