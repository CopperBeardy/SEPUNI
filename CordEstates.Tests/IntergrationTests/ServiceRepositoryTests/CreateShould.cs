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

namespace CordEstates.Tests.IntergrationTests.ServiceRepositoryTests
{
    public class CreateShould 
    {
        readonly DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddService() 
        {
            var service = setup.services.First();
            using (setup.context)
            {

                setup.serviceRepository.CreateService(service);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Services.Count());
                var obj = setup.contextConfirm.Services.First();
                Assert.Equal(service.ServiceName, obj.ServiceName);
            }
           
        }
  
    }
}
