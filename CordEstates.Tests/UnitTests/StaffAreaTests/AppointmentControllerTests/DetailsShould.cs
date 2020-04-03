using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.AppointmentControllerTests
{
    public class DetailsShould
    {
        private SetupFixture fixture;
        private ClaimsPrincipal claimsPrincipal;

        private AppointmentController sut;

        public DetailsShould()
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
                .Setup(x => x.User.GetUserId(claimsPrincipal))
                .ReturnsAsync(string.Empty);

            fixture.mapper.Setup(x => x.Map<AppointmentManagementDTO>(It.IsAny<Appointment>()))
                  .Returns(new AppointmentManagementDTO());
        }


        [Theory]
        [InlineData(2342)]
        [InlineData(2)]
        [InlineData(4)]
        public async void ReturnValidModelForCorrectId(int id)
        {

            var result = await sut.Details(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Appointment.GetAppointmentByIdAsync(id), Times.Once);
            Assert.Equal("Details", vr.ViewName);
            Assert.IsAssignableFrom<AppointmentManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(0)]

        [InlineData(-12)]
        public async void RedirectToIndexForInvalidId(int id)
        {
            var result = await sut.Details(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Appointment.GetAppointmentByIdAsync(id), Times.Never);

        }



    }
}
