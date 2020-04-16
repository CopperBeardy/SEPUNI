using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories;
using CordEstates.Tests.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace CordEstates.Tests.IntergrationTests.PhotoRepositoryTests
{
    public class UploadShould 
    {
        readonly DatabaseSetup setup;
        public UploadShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddPhoto() 
        {
            var photo = setup.photos.First();
            using (setup.context)
            {

                setup.photoRepository.UploadPhoto(photo);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Photos.Count());
                var obj = setup.contextConfirm.Photos.First();
                Assert.Equal(photo.ImageLink, obj.ImageLink);
            }
           
        }
  
    }
}
