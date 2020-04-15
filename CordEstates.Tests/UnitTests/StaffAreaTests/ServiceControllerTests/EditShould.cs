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
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly ServiceController sut;
        private readonly ServiceDTO service;

        public EditShould()
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
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Service.GetServiceByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<ServiceDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(-5434)]
        public async void RedirectToIndexForInvalidId(int? id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Service.GetServiceByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            int id = 2;

            var result = await sut.Edit(id, service);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EventDTO");

            var result = await sut.Edit(1, service);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Service.UpdateService(It.IsAny<Service>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<ServiceDTO>(vr.Model);
            Assert.IsAssignableFrom<ServiceDTO>(vr.Model);
        }



        [Fact]
        public async void UpdateModel()
        {

            var result = await sut.Edit(1, service);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Service.UpdateService(It.IsAny<Service>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void ThrowDbUpdateConncurrentExceptionWhenProblemUploading()
        {
            fixture.repositoryWrapper.Setup(x => x.Service.UpdateService(It.IsAny<Service>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1, service);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Service.UpdateService(It.IsAny<Service>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
