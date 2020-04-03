using CordEstates.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.RootControllerTests.EventControllerTess
{
    public class IndexShould
    {
        private EventController sut;

        private SetupFixture fixture;


        public IndexShould()
        {
            fixture = new SetupFixture();

            sut = new EventController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper.Setup(x => x.Event.GetAllEventsAsync()).ReturnsAsync(new List<Event>() { It.IsAny<Event>() });
            fixture.mapper.Setup(x => x.Map<List<EventDTO>>(It.IsAny<List<Event>>())).Returns(new List<EventDTO>());
        }



        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);

        }

        [Fact]
        public async Task ReturnListOfCurrentEvents()
        {


            var result = await sut.Index();

            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
            Assert.IsType<List<EventDTO>>(vr.Model);

        }

        [Fact]

        public async void ThrowExceptionIfEventNotFound()
        {
            fixture.repositoryWrapper.Setup(x => x.Event.GetAllEventsAsync()).Throws<Exception>();

            await Assert.ThrowsAsync<Exception>(() => sut.Index());


        }

    }
}
