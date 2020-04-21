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

namespace CordEstates.Tests.IntergrationTests.ListingRepositoryTests
{
    public class GetByIdShould 
    {
        readonly DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
            var address = setup.addresses.First();
            setup.context.Addresses.Add(address);
            setup.context.SaveChanges();

            var photo = setup.photos.First();
            setup.context.Photos.Add(photo);
            setup.context.SaveChanges();

             var listing = setup.listings.First();
            setup.context.Add(listing);
            setup.context.SaveChanges();

        }

        [Fact]
        public async void GetListingById() 
        {
            
            var id = setup.context.Listings.First().Id;
           var listing = await setup.listingRepository.GetListingByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Listings.First();            
                Assert.IsAssignableFrom<Listing>(listing);      
                Assert.IsAssignableFrom<Listing>(result);
                Assert.Equal(listing.Id, result.Id);
                Assert.Equal(listing.AddressId, result.AddressId);
                Assert.Equal(listing.ImageId, result.ImageId);


            }
           
        }
  
        [Fact]
        public void GetListingByAddressId()
        {

            var id = setup.context.Addresses.First().Id;
            var listing = setup.listingRepository.GetListingsIdByAddressID(id);


            using (setup.contextConfirm)
            {
                var result = setup.contextConfirm.Listings.First();
                Assert.IsAssignableFrom<Listing>(listing);
                Assert.IsAssignableFrom<Listing>(result);
                Assert.Equal(listing.Id, result.Id);
                Assert.Equal(listing.AddressId, result.AddressId);
                Assert.Equal(listing.ImageId, result.ImageId);


            }

        }
    }
}
