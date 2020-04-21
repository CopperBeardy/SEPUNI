using CordEstates.Areas.Staff.Controllers;
using CordEstates.Entities;
using CordEstates.Models.DTOs;
using CordEstates.Tests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using Xunit;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Tests.UnitTests.StaffAreaTests.PhotoControllerTests
{
    public class DeleteShould
    {
        private readonly SetupFixture fixture;
        private readonly PhotoController sut;
        private readonly Mock<IHostEnvironment> env;
        private readonly Mock<IImageUploadWrapper> imageUploadWrapper;
        private readonly Photo photo;
        private readonly PhotoDTO photoDTO;
        public DeleteShould()
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

            photo = new Photo() { Id = 1, ImageLink = "dgfdfgdf" };
            photoDTO = new PhotoDTO()
            {
                Id = 1,
                ImageLink = string.Empty,
                File = It.IsAny<IFormFile>()
            };

            fixture.repositoryWrapper.Setup(x => x.Photo.GetPhotoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(photo);
            fixture.mapper.Setup(x => x.Map<PhotoDTO>(It.IsAny<Photo>()))
                .Returns(photoDTO).Verifiable();

        }

        [Theory]
        [InlineData(21)]
        [InlineData(5)]
        [InlineData(157)]
        public async void ReturnValidModelForCorrectId(int id)
        {
            var result = await sut.Delete(id);
            var vr = Assert.IsType<ViewResult>(result);

            fixture.repositoryWrapper.Verify(y => y.Photo.GetPhotoByIdAsync(id), Times.Once);
            Assert.Equal("Delete", vr.ViewName);
            Assert.IsAssignableFrom<PhotoDTO>(vr.Model);
        }


        [Theory]

        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-12)]
        public async void RedirectToIndexIfInvalidValuePassed(int id)
        {
            var result = await sut.Delete(id);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            fixture.repositoryWrapper.Verify(y => y.Photo.GetPhotoByIdAsync(id), Times.Never);

        }


        [Fact]
        public async void CallDeletePhotoWithAValidModel()
        {
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Photo.DeletePhoto(It.IsAny<Photo>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Once);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

        }
        [Fact]
        public async void ThrowExceptionIfUnableToDeleteListing()
        {
            fixture.repositoryWrapper.Setup(x => x.Photo.DeletePhoto(It.IsAny<Photo>())).Throws(new Exception("error in the deletion process"));
            var result = await sut.DeleteConfirmed(It.IsAny<int>());

            fixture.repositoryWrapper.Verify(x => x.Photo.DeletePhoto(It.IsAny<Photo>()), Times.Once);
            fixture.repositoryWrapper.Verify(y => y.SaveAsync(), Times.Never);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }



    }
}
