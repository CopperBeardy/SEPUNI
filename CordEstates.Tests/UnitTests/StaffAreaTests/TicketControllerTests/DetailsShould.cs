using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.TicketControllerTests
{
    public class DetailsShould
    {
        private SetupFixture fixture;
        private TicketController sut;

        public DetailsShould()
        {

            fixture = new SetupFixture();

            sut = new TicketController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
             .Setup(x => x.Ticket.GetTicketByIdAsync(It.IsAny<string>()))
             .ReturnsAsync(new Ticket());
            fixture.mapper.Setup(x => x.Map<TicketManagementDTO>(It.IsAny<Ticket>())).Returns(new TicketManagementDTO());

        }


        [Theory]
        [InlineData("sdfsdf")]
        [InlineData("sdfsdgesrhge")]
        [InlineData("sdfsd")]
        public async void ReturnValidModelForCorrectId(string id)
        {

            var result = await sut.Details(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Ticket.GetTicketByIdAsync(id), Times.Once);
            Assert.Equal("Details", vr.ViewName);
            Assert.IsAssignableFrom<TicketManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        public async void RedirectToIndexForInvalidId(string id)
        {
            var result = await sut.Details(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Ticket.GetTicketByIdAsync(id), Times.Never);

        }



    }
}
