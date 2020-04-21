using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.TicketControllerTests
{
    public class EditShould
    {
        private readonly SetupFixture fixture;
        private readonly TicketController sut;

        public EditShould()
        {

            fixture = new SetupFixture();

            sut = new TicketController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
             .Setup(x => x.Ticket.GetTicketByIdAsync(It.IsAny<string>()))
             .ReturnsAsync(new Ticket());
            fixture.mapper.Setup(x => x.Map<TicketManagementDTO>(It.IsAny<Ticket>()))
                .Returns(new TicketManagementDTO());

        }


        [Theory]
        [InlineData("sdfsdf")]
        [InlineData("sdfsdgesrhge")]
        [InlineData("sdfsd")]
        public async void ReturnValidModelForCorrectId(string id)
        {

            var result = await sut.Edit(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Ticket.GetTicketByIdAsync(id), Times.Once);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsAssignableFrom<TicketManagementDTO>(vr.Model);
        }

        [Theory]
        [InlineData(null)]
        public async void RedirectToIndexForInvalidId(string id)
        {
            var result = await sut.Edit(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Ticket.GetTicketByIdAsync(id), Times.Never);

        }

        [Fact]
        public async void RedirectToIndexIfIdsDontMatchOnPost()
        {
            string id = "some_string";
            TicketManagementDTO tm = new TicketManagementDTO() { Id = "notMatching" };

            var result = await sut.Edit(id, tm);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid TicketManagementDTO");

            string id = "Matching";
            TicketManagementDTO tm = new TicketManagementDTO() { Id = "Matching" };

            var result = await sut.Edit(id, tm);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Ticket.UpdateTicket(It.IsAny<Ticket>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("Edit", vr.ViewName);
            Assert.IsType<TicketManagementDTO>(vr.Model);

        }


        [Fact]
        public async void CallUpdateTicketInRepository()
        {

            string id = "Matching";
            TicketManagementDTO tm = new TicketManagementDTO() { Id = "Matching",Actioned=true };

            var result = await sut.Edit(id, tm);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Ticket.UpdateTicket(It.IsAny<Ticket>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
        }

        [Fact]
        public async void ThrowExceptionWhenProblemUploading()
        {
            fixture.repositoryWrapper.Setup(x => x.Event.UpdateEvent(It.IsAny<Event>())).Throws(new Exception("error in the update process"));


            var result = await sut.Edit(It.IsAny<string>());
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            fixture.repositoryWrapper.Verify(x => x.Ticket.UpdateTicket(It.IsAny<Ticket>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);

        }

    }
}
