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
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
      

        private readonly AppointmentController sut;

        public DeleteShould()
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
                .Setup(x => x.Employee.GetUserId(new ClaimsPrincipal()))
                .ReturnsAsync(string.Empty);

            fixture.mapper.Setup(x => x.Map<AppointmentManagementDTO>(It.IsAny<Appointment>()))
                  .Returns(new AppointmentManagementDTO());
        }

        [Theory]
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int? id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Appointment.GetAppointmentByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<AppointmentManagementDTO>(vr.Model);
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
            fixture.repositoryWrapper.Verify(y => y.Appointment.GetAppointmentByIdAsync(id), Times.Never);

        }
        [Fact]
        public async void CallDeleteAppointmentWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Appointment.DeleteAppointment(It.IsAny<Appointment>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteService()
        {
            fixture.repositoryWrapper.Setup(x => x.Appointment.DeleteAppointment(It.IsAny<Appointment>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Appointment.DeleteAppointment(It.IsAny<Appointment>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
