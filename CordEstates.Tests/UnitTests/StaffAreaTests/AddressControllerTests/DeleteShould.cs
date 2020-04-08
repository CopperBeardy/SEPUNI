using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AddressControllerTests
{
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
        private readonly AddressController sut;
        private readonly Address address;

        public DeleteShould()
        {

            fixture = new SetupFixture();

            sut = new AddressController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);
            address = new Address() { Id = 1, FirstLine = "FirstLine", SecondLine = "SecondLine" };
            fixture.repositoryWrapper.Setup(x => x.Address.DeleteAddress(It.IsAny<Address>())).Verifiable();

            fixture.repositoryWrapper.Setup(x => x.Address.GetAddressByIdAsync(It.IsAny<int>())).ReturnsAsync(address);

            fixture.mapper.Setup(x => x.Map<AddressDTO>(It.IsAny<Address>())).Returns(new AddressDTO());

        }

        [Theory]
        [InlineData(2)]
        [InlineData(75)]
        [InlineData(57)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Address.GetAddressByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<AddressDTO>(vr.Model);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-2234)]
        public async void RedirectToIndexIfInvalidValuePassed(int? id)
        {
            var result = await sut.Delete(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Address.GetAddressByIdAsync(id), Times.Never);



        }
        [Fact]
        public async void CallDeleteAddressModelWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Address.DeleteAddress(It.IsAny<Address>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteAddress()
        {
            fixture.repositoryWrapper.Setup(x => x.Address.DeleteAddress(It.IsAny<Address>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Address.DeleteAddress(It.IsAny<Address>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
