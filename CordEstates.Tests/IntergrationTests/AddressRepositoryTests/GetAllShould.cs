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

namespace CordEstates.Tests.IntergrationTests.AddressRepositoryTests
{
    public class GetAllShould
    {
        DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfAddress()
        {

            foreach (Address Address in setup.addresses)
            {
                setup.context.Addresses.Add(Address);
            }
            setup.context.SaveChanges();

            var result = await setup.addressRepository.GetAllAddressesAsync();

            using (setup.contextConfirm)
            {

                Assert.Equal(3, setup.contextConfirm.Addresses.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Address>>(result);
            }

        }

        [Fact]
        public async void ReturnListOfAddressNotInUse()
        {

            foreach (Address Address in setup.addresses)
            {
                setup.context.Addresses.Add(Address);
            }
            setup.context.SaveChanges();

            var listing = setup.listings[0];
            listing.Address = setup.context.Addresses.First();


            setup.context.Listings.Add(listing);
            setup.context.SaveChanges();

            var result = await setup.addressRepository.GetAllAddressesNotInUseAsync();

            using (setup.contextConfirm)
            {

                Assert.Equal(3, setup.contextConfirm.Addresses.Count());
                Assert.Equal(2, result.Count);
                Assert.IsAssignableFrom<List<Address>>(result);
            }

        }
    }
}
