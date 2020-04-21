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

namespace CordEstates.Tests.IntergrationTests.ServiceRepositoryTests
{
    public class GetAllShould 
    {
        readonly DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfService()
        {
           

           
            foreach (Service service in setup.services)
            {
                
                setup.context.Services.Add(service);
            }
              setup.context.SaveChanges();  
            
            var result = await setup.serviceRepository.GetAllServicesAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Services.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Service>>(result);
            }
           
        }

      

        
    }
}
