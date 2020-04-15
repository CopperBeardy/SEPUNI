using CordEstates.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.RootControllerTests.ServiceControllerTests
{
    public class IndexShould
    {

        private readonly ServiceController sut;
        readonly SetupFixture fixture;

        public IndexShould()
        {
            fixture = new SetupFixture();
            fixture.context.Setup(_ => _.Set<Service>()).Returns(new Mock<DbSet<Service>>().Object);
            sut = new ServiceController(fixture.Logger.Object,
                fixture.mapper.Object,
               fixture.repositoryWrapper.Object);
            fixture.repositoryWrapper.Setup(x => x.Service.GetAllServicesAsync()).ReturnsAsync(new List<Service>() { It.IsAny<Service>() });
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
        public async Task ReturnListOfCurrentServicesOffered()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
            Assert.IsType<List<ServiceDTO>>(vr.Model);
        }

        [Fact]

        public async void ThrowExceptionIfUnableToRetrieveServices()
        {
            fixture.repositoryWrapper.Setup(x => x.Service.GetAllServicesAsync()).Throws<Exception>();
            await Assert.ThrowsAsync<Exception>(() => sut.Index());
        }

    }
}
