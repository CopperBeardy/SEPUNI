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
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CordEstates.Tests.IntergrationTests.BuyerRepositoryTests
{
    public class UpdateShould 
    {
        DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateBuyer() 
        {
           
            setup.context.Add(setup.buyers.First());
            setup.context.SaveChanges();

            var Buyer = setup.context.Buyers.First();

            Buyer.FirstName = "Change of text";
            setup.buyerRepository.UpdateBuyer(Buyer);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Buyers.First();            
                
                Assert.Equal("Change of text", result.FirstName);
            }
           
        }
  
    }
}
