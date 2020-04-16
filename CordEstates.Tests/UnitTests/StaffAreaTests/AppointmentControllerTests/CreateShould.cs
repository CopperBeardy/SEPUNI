using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AppointmentControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly AppointmentController sut;

        public CreateShould()
        {
            fixture = new SetupFixture();

            sut = new AppointmentController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object

              );

            fixture.repositoryWrapper
        .Setup(x => x.User.GetUserId(new ClaimsPrincipal()))
        .ReturnsAsync(string.Empty);
            fixture.repositoryWrapper
                .Setup(x => x.Appointment.GetAppointmentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Appointment());
            fixture.repositoryWrapper
                .Setup(x => x.Listing.GetAllListingsAsync())
                .ReturnsAsync(new List<Listing>());

      

            fixture.mapper.Setup(x => x.Map<CreateAppointmentDTO>(It.IsAny<Appointment>()))
                  .Returns(new CreateAppointmentDTO());
        }

        [Fact]
        public async void ReturnCreateView()
        {
            var result = await sut.Create();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Create", vr.ViewName);
        }


        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid AppointmentDTO");

            CreateAppointmentDTO createAppointmentDTO = new CreateAppointmentDTO() { };
            var result = await sut.Create(createAppointmentDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Appointment.CreateAppointment(It.IsAny<Appointment>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Create", vr.ViewName);
            Assert.IsType<CreateAppointmentDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateAppointmentInRepository()
        {


            CreateAppointmentDTO createAppointmentDTO = new CreateAppointmentDTO() { StaffId = "a test", ListingId = 1, Time = It.IsAny<DateTime>() };
            var result = await sut.Create(createAppointmentDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Appointment.CreateAppointment(It.IsAny<Appointment>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);


        }


    }
}
