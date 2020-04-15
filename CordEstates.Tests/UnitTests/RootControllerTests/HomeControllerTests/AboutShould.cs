using CordEstates.Areas.Identity.Data;
using CordEstates.Controllers;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CordEstates.Tests.UnitTests.RootControllerTests.HomeControllerTests
{

    public class AboutShould
    {
        private readonly HomeController sut;
        private readonly SetupFixture fixture;

        public AboutShould()
        {
            fixture = new SetupFixture();

            sut = new HomeController(fixture.Logger.Object,
                fixture.mapper.Object,
                fixture.repositoryWrapper.Object);
            fixture.repositoryWrapper.Setup(x => x.User.GetAllStaffAsync()).ReturnsAsync(It.IsAny<List<ApplicationUser>>());
            fixture.mapper.Setup(x => x.Map<List<UserDTO>>(It.IsAny<List<UserDTO>>())).Returns(new List<UserDTO>());

        }

        [Fact]
        public async Task RequestApplicationUsersThatAreInTheStaffRolesFromDataBase()
        {
            var result = await sut.About();
            var vm = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<UserDTO>>(vm.Model);
        }
    }

}
