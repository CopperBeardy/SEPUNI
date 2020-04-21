using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.ServiceControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;

        private readonly ServiceController sut;

        public CreateShould()
        {
            fixture = new SetupFixture();

            sut = new ServiceController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
               .Setup(x => x.Service.GetServiceByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Service());
            
            fixture.mapper.Setup(x => x.Map<ServiceDTO>(It.IsAny<Service>()))
                .Returns(new ServiceDTO());

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

            sut.ModelState.AddModelError(string.Empty, "Invalid ServiceDTO");

            ServiceDTO ServiceDTO = new ServiceDTO() { Id = 1 };
            var result = await sut.Create(ServiceDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Service.CreateService(It.IsAny<Service>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<ServiceDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateServiceInRepository()
        {


            ServiceDTO ServiceDTO = new ServiceDTO() { Id = 1 };
            var result = await sut.Create(ServiceDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Service.CreateService(It.IsAny<Service>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);


        }


    }
}
