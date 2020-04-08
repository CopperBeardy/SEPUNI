using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.SetupFixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Collections.Generic;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.PhotoControllerTests
{
    public class CreateShould
    {
        private readonly SetupFixture fixture;
        private readonly PhotoController sut;
        private readonly Mock<IHostEnvironment> env;
        private readonly Mock<IImageUploadWrapper> imageUploadWrapper;

        private readonly Photo photo;
        private readonly PhotoDTO photoDTO;
        public CreateShould()
        {
            fixture = new SetupFixture();

            env = new Mock<IHostEnvironment>();
            imageUploadWrapper = new Mock<IImageUploadWrapper>();
            env.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            sut = new PhotoController(fixture.Logger.Object,
                                        fixture.repositoryWrapper.Object,
                                        fixture.mapper.Object,
                                        env.Object,
                                        imageUploadWrapper.Object);

            photo = new Photo()
            { Id = 1, ImageLink = "dgfdfgdf" };
            photoDTO = new PhotoDTO() { Id = 1, ImageLink = string.Empty, File = It.IsAny<IFormFile>() };


            fixture.repositoryWrapper
                    .Setup(x => x.Photo.GetPhotoByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(It.IsAny<Photo>);
            fixture.repositoryWrapper.Setup(x => x.Photo.GetAllPhotosAsync()).ReturnsAsync(new List<Photo>() { photo });
            fixture.mapper.Setup(x => x.Map<PhotoDTO>(It.IsAny<Photo>())).Returns(new PhotoDTO());
            imageUploadWrapper.Setup(x => x.Upload(It.IsAny<IFormFile>(), It.IsAny<IHostEnvironment>()))
                .Returns("imageurl");


        }
        [Fact]
        public async void ReturnCreateView()
        {
            var result = sut.CreatePhoto();
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal("CreatePhoto", vr.ViewName);
        }


        [Fact]
        public async void ReturnViewIfModelStateInvalid()
        {

            sut.ModelState.AddModelError(string.Empty, "Invalid UploadEventDTO");

            var result = await sut.CreatePhoto(photoDTO);

            var vr = Assert.IsType<ViewResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Photo.UploadPhoto(It.IsAny<Photo>()), Times.Never);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.Equal("CreatePhoto", vr.ViewName);
            Assert.IsType<PhotoDTO>(vr.Model);
        }


        [Fact]
        public async void CallCreateListingInRepository()
        {



            fixture.repositoryWrapper.Setup(x => x.Photo.Create(It.IsAny<Photo>()));

            var result = await sut.CreatePhoto(photoDTO);

            var vr = Assert.IsType<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(x => x.Photo.Create(It.IsAny<Photo>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.Equal("Index", vr.ActionName);

        }


    }
}
