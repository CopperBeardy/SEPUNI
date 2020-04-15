using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AgentDashboardControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly AgentDashboardController sut;
        private readonly ClaimsPrincipal claimsPrincipal;


        public IndexShould()
        {
            fixture = new SetupFixture();

            sut = new AgentDashboardController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object
              );
            fixture.repositoryWrapper
             .Setup(x => x.User.GetUserId(claimsPrincipal))
             .ReturnsAsync(string.Empty);
            fixture.repositoryWrapper
             .Setup(x => x.Appointment.GetAllAppointmentsByStaffIdAsync(It.IsAny<string>()))
             .ReturnsAsync(new List<Appointment>() { It.IsAny<Appointment>() });
            fixture.mapper.Setup(x => x.Map<List<AppointmentDTO>>(It.IsAny<List<Appointment>>())).Returns(new List<AppointmentDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllAppointmentsForStaffID()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<AppointmentDTO>>(vr.Model);

        }



    }
}
