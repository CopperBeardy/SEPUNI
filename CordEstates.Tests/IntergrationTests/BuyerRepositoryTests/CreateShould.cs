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
    public class CreateShould 
    {
        DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddBuyer() 
        {
            var buyer = setup.buyers.First();
            using (setup.context)
            {

                setup.buyerRepository.CreateBuyer(buyer);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Buyers.Count());
                var obj = setup.contextConfirm.Buyers.First();
                Assert.Equal(buyer.FirstName, obj.FirstName);
            }
           
        }
  
    }
}
