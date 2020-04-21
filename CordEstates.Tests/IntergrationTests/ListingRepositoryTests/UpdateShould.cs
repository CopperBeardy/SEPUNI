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

namespace CordEstates.Tests.IntergrationTests.ListingRepositoryTests
{
    public class UpdateShould 
    {
        readonly DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateListing() 
        {
           
            setup.context.Add(setup.listings.First());
            setup.context.SaveChanges();

            var listing = setup.context.Listings.First();

            listing.Description = "Change of text";
            listing.ImageId = 2;
            setup.listingRepository.UpdateListing(listing);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Listings.First();            
                
                Assert.Equal("Change of text", result.Description);
                Assert.Equal(2, result.ImageId);
            }
           
        }
  
    }
}
