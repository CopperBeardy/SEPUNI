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
    public class GetByIdShould 
    {
        readonly DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetPhotoById() 
        {
           var p = setup.photos.First();
           setup.context.Add(p);


            setup.context.SaveChanges();

            var id = setup.context.Photos.First().Id;
           
           var Photo = await setup.photoRepository.GetPhotoByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Photos.First();            
                Assert.IsAssignableFrom<Photo>(Photo);
                Assert.IsAssignableFrom<Photo>(result);
                Assert.Equal(Photo.Id, result.Id);
                Assert.Equal(Photo.ImageLink, result.ImageLink);

            }
           
        }
  
    }
}
