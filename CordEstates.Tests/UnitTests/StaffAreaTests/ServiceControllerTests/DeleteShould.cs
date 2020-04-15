using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ServiceControllerTests
{
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
        private readonly ServiceController sut;
        private readonly ServiceDTO service;

        public DeleteShould()
        {
            fixture = new SetupFixture();

            sut = new ServiceController(fixture.Logger.Object, fixture.repositoryWrapper.Object, fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Service.GetServiceByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Service());
            fixture.mapper.Setup(x => x.Map<ServiceDTO>(It.IsAny<Service>())).Returns(new ServiceDTO());

            service = new ServiceDTO() { Id = 1, };
        }

        [Theory]
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Service.GetServiceByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<ServiceDTO>(vr.Model);
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
            fixture.repositoryWrapper.Verify(y => y.Service.GetServiceByIdAsync(id), Times.Never);

        }
        [Fact]
        public async void CallDeleteEventModelWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Service.DeleteService(It.IsAny<Service>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteService()
        {
            fixture.repositoryWrapper.Setup(x => x.Service.DeleteService(It.IsAny<Service>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(service.Id);

            fixture.repositoryWrapper.Verify(x => x.Service.DeleteService(It.IsAny<Service>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
