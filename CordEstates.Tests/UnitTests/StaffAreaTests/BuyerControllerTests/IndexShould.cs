using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.BuyerControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly BuyerController sut;
  
        public IndexShould()
        {

            fixture = new SetupFixture();
      
            sut = new BuyerController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Buyer.GetAllBuyersAsync())
                .ReturnsAsync(new List<Buyer>() { It.IsAny<Buyer>() });
            
            fixture.mapper.Setup(x => x.Map<List<BuyerManagementDTO>>(It.IsAny<List<Buyer>>())).
                    Returns(new List<BuyerManagementDTO>() { It.IsAny<BuyerManagementDTO>()});

        }

        [Fact]
        public async void ReturnCorrectView()
        {
            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", vr.ViewName);
        }


        [Fact]
        public async Task ReturnListOfAllEvents()
        {

            var result = await sut.Index();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<BuyerManagementDTO>>(vr.Model);

        }



    }
}
