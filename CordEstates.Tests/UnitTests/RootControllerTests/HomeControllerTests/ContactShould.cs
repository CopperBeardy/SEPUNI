using CordEstates.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace CordEstates.Tests.UnitTests.RootControllerTests.HomeControllerTests
{
    public class ContactShould
    {

        private readonly HomeController sut;
        private readonly SetupFixture fixture;

        public ContactShould()
        {
            fixture = new SetupFixture();
            sut = new HomeController(fixture.Logger.Object,
                fixture.mapper.Object,
                fixture.repositoryWrapper.Object);

            fixture.repositoryWrapper.Setup(h => h.Ticket.CreateTicket(It.IsAny<Ticket>()));
        }


        [Fact]
        public void ShouldCallAddTicket()
        {
             sut.Contact(It.IsAny<CreateTicketDTO>());
            fixture.repositoryWrapper.Verify(x => x.Ticket.CreateTicket(It.IsAny<Ticket>()));
        }

        [Fact]
        public  void RedirectOnSuccessfulAddingTicketToDatabase()
        {
            var result =  sut.Contact(It.IsAny<CreateTicketDTO>());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Ticket.CreateTicket(It.IsAny<Ticket>()));
            Assert.Equal("Index", redirect.ActionName);
        }


        [Fact]
        public void ReturnContactViewOnInvalidModelState()
        {
            sut.ModelState.AddModelError("FirstName", "Required");
            var result =  sut.Contact(It.IsAny<CreateTicketDTO>());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Contact", viewResult.ViewName);
        }

        [Fact]

        public  void ThrowExceptionIfErrorEncounterAddingTicketToDatabase()
        {
            fixture.repositoryWrapper.Setup(x => x.Ticket.CreateTicket(It.IsAny<Ticket>())).Throws<Exception>();
            Assert.Throws<Exception>(() => sut.Contact(It.IsAny<CreateTicketDTO>()));
        }
    }
}
