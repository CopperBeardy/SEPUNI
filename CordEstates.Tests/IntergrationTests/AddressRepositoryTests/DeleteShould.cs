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
    public class DeleteShould 
    {
        DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteAddress() 
        {
            var Address = setup.addresses.First();
            using (setup.context)
            {
                //add Address to the db
                setup.addressRepository.CreateAddress(Address);
                setup.context.SaveChanges();
                // remove Address
                setup.addressRepository.DeleteAddress(Address);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Addresses.Count());
           
            }
           
        }
  
    }
}
