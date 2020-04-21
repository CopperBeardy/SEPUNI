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

namespace CordEstates.Tests.IntergrationTests.ServiceRepositoryTests
{
    public class DeleteShould 
    {
        readonly DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteService() 
        {
            var service = setup.services.First();
            using (setup.context)
            {
                //add Service to the db
                setup.serviceRepository.CreateService(service);
                setup.context.SaveChanges();
                // remove Service
                setup.serviceRepository.DeleteService(service);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Services.Count());
           
            }
           
        }
  
    }
}
