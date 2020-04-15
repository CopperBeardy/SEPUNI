using CordEstates.Areas.Staff.Controllers;
using CordEstates.Areas.Staff.Models.DTOs;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace CordEstates.Tests.UnitTests.StaffAreaTests.SaleControllerTests
{
    public class IndexShould
    {
        private readonly SetupFixture fixture;
        private readonly SaleController sut;
  
        public IndexShould()
        {

            fixture = new SetupFixture();
      
            sut = new SaleController(fixture.Logger.Object,
                fixture.repositoryWrapper.Object,
                fixture.mapper.Object);

            fixture.repositoryWrapper
                .Setup(x => x.Sale.GetAllSalesAsync())
                .ReturnsAsync(new List<Sale>() { It.IsAny<Sale>() });
            
            fixture.mapper.Setup(x => x.Map<List<SaleManagementDTO>>(It.IsAny<List<Sale>>())).
                    Returns(new List<SaleManagementDTO>() { It.IsAny<SaleManagementDTO>()});

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
            Assert.IsAssignableFrom<List<SaleManagementDTO>>(vr.Model);

        }



    }
}
