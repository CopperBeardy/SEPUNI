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
    public class DeleteShould 
    {
        readonly DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeletePhoto() 
        {
            var photo = setup.photos.First();
            using (setup.context)
            {
                //add Photo to the db
                setup.photoRepository.UploadPhoto(photo);
                setup.context.SaveChanges();
                // remove Photo
                setup.photoRepository.DeletePhoto(photo);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Photos.Count());
           
            }
           
        }
  
    }
}
