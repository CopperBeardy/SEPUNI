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
    public class DeleteShould 
    {
        readonly DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteListing() 
        {
            var Listing = setup.listings.First();
            using (setup.context)
            {
                //add Listing to the db
                setup.listingRepository.CreateListing(Listing);
                setup.context.SaveChanges();
                // remove Listing
                setup.listingRepository.DeleteListing(Listing);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Listings.Count());
           
            }
           
        }
  
    }
}
