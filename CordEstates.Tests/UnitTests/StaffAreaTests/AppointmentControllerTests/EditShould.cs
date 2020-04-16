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
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly AppointmentController sut;
       
        private readonly EditAppointmentManagementDTO app;

        public EditShould()
        {
            fixture = new SetupFixture();

            sut = new AppointmentController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object
              );

            fixture.repositoryWrapper
                 .Setup(x => x.Appointment.GetAppointmentByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(new Appointment());
            fixture.repositoryWrapper
                .Setup(x => x.Listing.GetAllListingsAsync())
                .ReturnsAsync(new List<Listing>());

    
            fixture.repositoryWrapper
            .Setup(x => x.Listing.GetAllListingsAsync())
            .ReturnsAsync(new List<Listing>());

            fixture.mapper.Setup(x => x.Map<EditAppointmentManagementDTO>(It.IsAny<Appointment>()))
                    .Returns(new EditAppointmentManagementDTO());
            app = new EditAppointmentManagementDTO() { Id = 1 };
        }
        [Theory]
        [InlineData(34)]
        [InlineData(2334)]
        [InlineData(5434)]
        public async void ReturnValidModelForCorrectId(int? id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Appointment.GetAppointmentByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<EditAppointmentManagementDTO>(vr.Model);
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

            var result = await sut.Edit(id, app);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid EditAppointmentDTO");

            var result = await sut.Edit(1, app);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Appointment.UpdateAppointment(It.IsAny<Appointment>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<EditAppointmentManagementDTO>(vr.Model);
            Assert.IsAssignableFrom<EditAppointmentManagementDTO>(vr.Model);
        }



        [Fact]
        public async void UpdateModel()
        {

            var result = await sut.Edit(1, app);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Appointment.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);

        }

        [Fact]
        public async void ThrowDbUpdateConcurrentExceptionWhenUpdating()
        {
            fixture.repositoryWrapper.Setup(x => x.Appointment.UpdateAppointment(It.IsAny<Appointment>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(1, app);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Appointment.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
