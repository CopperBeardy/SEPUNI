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

namespace CordEstates.Tests.IntergrationTests.ListingRepositoryTests
{
    public class GetAllShould 
    {
        readonly DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
            //create address
            foreach (Address address in setup.addresses)
            {
                setup.context.Addresses.Add(address);
            }
            setup.context.SaveChanges();

            foreach(Photo photo in setup.photos)
            {
                setup.context.Photos.Add(photo);
            }
            setup.context.SaveChanges();

            var photos = setup.context.Photos.ToList();
            var addresses = setup.context.Addresses.ToList();
            //create listing and associate with address
            for (int i = 0; i < 3; i++)
            {
                var listing = setup.listings[i];
                listing.AddressId = addresses[i].Id;
                listing.ImageId = photos[i].Id;
                setup.context.Listings.Add(listing);

            }
            setup.context.SaveChanges();
        }
        [Fact]
        public async void ReturnListOfListing()
        {         
            var result = await setup.listingRepository.GetAllListingsAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Listings.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Listing>>(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void ReturnListOfListingForLandingPage(int amount)
        {
            var result = await setup.listingRepository.GetLandingPageListingsAsync(amount);

            using (setup.contextConfirm)
            {

                Assert.Equal(3, setup.contextConfirm.Listings.Count());
                Assert.Equal(amount, result.Count);
                Assert.IsAssignableFrom<List<Listing>>(result);
            }
        }


        [Fact]
        public async void ReturnListOfListingsForSale()
        {
            var result = await setup.listingRepository.GetAllListingsForSaleAsync();

            using (setup.contextConfirm)
            {

                Assert.Equal(3, setup.contextConfirm.Listings.Count());
                Assert.Equal(2, result.Count);
                Assert.IsAssignableFrom<List<Listing>>(result);
            }
        }

    }
}
