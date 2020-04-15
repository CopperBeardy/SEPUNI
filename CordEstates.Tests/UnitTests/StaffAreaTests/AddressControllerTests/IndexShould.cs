using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AddressControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly AddressController sut;

        public IndexShould()
        {

            fixture = new SetupFixture();

            sut = new AddressController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
             .Setup(x => x.Address.GetAllAddressesAsync())
             .ReturnsAsync(new List<Address>() { It.IsAny<Address>() });
            fixture.mapper.Setup(x => x.Map<List<AddressDTO>>(It.IsAny<List<Address>>())).Returns(new List<AddressDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);

        }


        [Fact]
        public async Task ReturnListOfAllAddress()
        {


            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<AddressDTO>>(vr.Model);


        }
    }
}
