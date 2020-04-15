using CordEstates.Controllers;
using CordEstates.Entities;
using CordEstates.Models.ViewModels;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;


namespace CordEstates.Tests.UnitTests.RootControllerTests.HomeControllerTests
{
    public class IndexShould
    {

        private readonly HomeController sut;
        private readonly SetupFixture fixture;

        public IndexShould()
        {
            fixture = new SetupFixture();

            sut = new HomeController(fixture.Logger.Object,
                fixture.mapper.Object,
                fixture.repositoryWrapper.Object
                );
            fixture.repositoryWrapper.Setup(x => x.Service.GetAllServicesAsync()).ReturnsAsync(new List<Service>() { It.IsAny<Service>() });
            fixture.repositoryWrapper.Setup(x => x.Listing.GetAllListingsAsync()).ReturnsAsync(new List<Listing>() { It.IsAny<Listing>() });
            fixture.repositoryWrapper.Setup(x => x.Event.GetActiveEventAsync()).ReturnsAsync(It.IsAny<Event>);
        }


        [Fact]
        public async void ReturnCorrectView()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);

        }

        [Fact]
        public async void CallGetAllServicesOnce()
        {

            await sut.Index();
            fixture.repositoryWrapper.Verify(s => s.Service.GetAllServicesAsync(), Times.Once);
        }

        [Fact]
        public async void CallGetEventOnce()
        {
            await sut.Index();
            fixture.repositoryWrapper.Verify(s => s.Event.GetActiveEventAsync(), Times.Once);
        }

        [Fact]
        public async void CallGetAllListingsOnce()
        {
            await sut.Index();
            fixture.repositoryWrapper.Verify(s => s.Listing.GetLandingPageListingsAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void ReturnIndexViewModel()
        {
            var result = await sut.Index();
            var vm = Assert.IsType<ViewResult>(result);
            Assert.IsType<LandingPageViewModel>(vm.Model);
            Assert.IsAssignableFrom<LandingPageViewModel>(vm.Model);
        }

        [Fact]

        public async void ThrowExceptionIfIndexViewModelCreationEncountersAProblems()
        {
            fixture.repositoryWrapper.Setup(x => x.Service.GetAllServicesAsync()).Throws<Exception>();

            await Assert.ThrowsAsync<Exception>(() => sut.Index());



        }

    }
}
