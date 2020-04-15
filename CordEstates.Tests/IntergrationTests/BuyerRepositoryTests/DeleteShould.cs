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

namespace CordEstates.Tests.IntergrationTests.BuyerRepositoryTests
{
    public class DeleteShould
    {
        DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void DeleteBuyer()
        {
            var buyer = setup.buyers.First();
            using (setup.context)
            {
                //add Buyer to the db
                setup.buyerRepository.CreateBuyer(buyer);
                setup.context.SaveChanges();
                // remove Buyer
                setup.buyerRepository.DeleteBuyer(buyer);
                setup.context.SaveChanges();
            }



            using (setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Buyers.Count());

            }

        }

    }
}
