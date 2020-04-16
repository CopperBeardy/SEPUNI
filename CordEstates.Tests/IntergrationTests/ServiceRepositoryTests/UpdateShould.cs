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
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CordEstates.Tests.IntergrationTests.ServiceRepositoryTests
{
    public class UpdateShould 
    {
        readonly DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateService() 
        {
           
            setup.context.Add(setup.services.First());
            setup.context.SaveChanges();

            var service = setup.context.Services.First();

            service.ServiceName = "Change of text";
            setup.serviceRepository.UpdateService(service);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Services.First();            
                
                Assert.Equal("Change of text", result.ServiceName);
            }
           
        }
  
    }
}
