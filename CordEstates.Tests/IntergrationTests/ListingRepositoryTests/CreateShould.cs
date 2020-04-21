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
    public class CreateShould 
    {
        readonly DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddListing() 
        {
            var Listing = setup.listings.First();
            using (setup.context)
            {

                setup.listingRepository.CreateListing(Listing);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Listings.Count());
                var obj = setup.contextConfirm.Listings.First();
                Assert.Equal(Listing.Description, obj.Description);
            }
           
        }
  
    }
}
