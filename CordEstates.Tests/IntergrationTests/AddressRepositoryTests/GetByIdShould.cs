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
    public class GetByIdShould 
    {
        DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetAddressById() 
        {
           
            setup.context.Add(setup.addresses.First());
            setup.context.SaveChanges();

            var id = setup.context.Addresses.First().Id;
           
           var Address = await setup.addressRepository.GetAddressByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Addresses.First();            
                Assert.IsAssignableFrom<Address>(Address);
                Assert.IsAssignableFrom<Address>(result);
                Assert.Equal(Address.Id, result.Id);
            
            }
           
        }
  
    }
}
