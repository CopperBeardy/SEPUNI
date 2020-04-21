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

namespace CordEstates.Tests.IntergrationTests.AddressRepositoryTests
{
    public class UpdateShould 
    {
        DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateAddress() 
        {
           
            setup.context.Add(setup.addresses.First());
            setup.context.SaveChanges();

            var Address = setup.context.Addresses.First();

            Address.FirstLine = "Change of text";
            setup.addressRepository.UpdateAddress(Address);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Addresses.First();            
                
                Assert.Equal("Change of text", result.FirstLine);
            }
           
        }
  
    }
}
