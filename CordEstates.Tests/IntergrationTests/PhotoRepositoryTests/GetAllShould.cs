using CordEstates.Entities;
using CordEstates.Repositories;
using CordEstates.Tests.Setup;
using Microsoft.CodeAnalysis.CSharp;
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
    public class GetAllShould 
    {
        readonly DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfPhoto()
        {
           

           
            foreach (Photo photo in setup.photos)
            {
                
                setup.context.Photos.Add(photo);
            }
              setup.context.SaveChanges();  
            
            var result = await setup.photoRepository.GetAllPhotosAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Photos.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Photo>>(result);
            }
           
        }

      

        
    }
}
