using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.AppointmentControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly AppointmentController sut;


        public IndexShould()
        {
            fixture = new SetupFixture();

            sut = new AppointmentController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object
              );

            fixture.repositoryWrapper
             .Setup(x => x.Appointment.GetAllAppointmentsAsync())
             .ReturnsAsync(new List<Appointment>() { It.IsAny<Appointment>() });
            fixture.mapper.Setup(x => x.Map<List<AppointmentManagementDTO>>(It.IsAny<List<Appointment>>())).Returns(new List<AppointmentManagementDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllAppointments()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<AppointmentManagementDTO>>(vr.Model);

        }



    }
}
