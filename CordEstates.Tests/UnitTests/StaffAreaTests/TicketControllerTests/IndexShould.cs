using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.TicketControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly TicketController sut;

        public IndexShould()
        {

            fixture = new SetupFixture();

            sut = new TicketController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
             .Setup(x => x.Ticket.GetAllTicketsAsync())
             .ReturnsAsync(new List<Ticket>() { It.IsAny<Ticket>() });
            fixture.mapper.Setup(x => x.Map<List<TicketManagementDTO>>(It.IsAny<List<Ticket>>())).Returns(new List<TicketManagementDTO>());

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllTickets()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<TicketManagementDTO>>(vr.Model);

        }



    }
}
