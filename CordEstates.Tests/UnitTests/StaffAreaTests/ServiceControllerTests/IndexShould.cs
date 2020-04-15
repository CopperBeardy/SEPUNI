using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ServiceControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly ServiceController sut;

        public IndexShould()
        {
            fixture = new SetupFixture();

            sut = new ServiceController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object
              );

            fixture.repositoryWrapper
             .Setup(x => x.Service.GetAllServicesAsync())
             .ReturnsAsync(new List<Service>() { It.IsAny<Service>() });
            fixture.mapper.Setup(x => x.Map<List<ServiceDTO>>(It.IsAny<List<Service>>())).Returns(new List<ServiceDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllEvents()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ServiceDTO>>(vr.Model);

        }



    }
}
