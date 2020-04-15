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

namespace CordEstates.Tests.IntergrationTests.AddressRepositoryTests
{
    public class CreateShould 
    {
        DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddAddress() 
        {
            var Address = setup.addresses.First();
            using (setup.context)
            {

                setup.addressRepository.CreateAddress(Address);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Addresses.Count());
                var obj = setup.contextConfirm.Addresses.First();
                Assert.Equal(Address.FirstLine, obj.FirstLine);
            }
           
        }
  
    }
}
