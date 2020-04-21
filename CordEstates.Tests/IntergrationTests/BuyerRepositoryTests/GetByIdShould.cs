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
    public class GetByIdShould 
    {
        DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetBuyerById() 
        {
           
            setup.context.Add(setup.buyers.First());
            setup.context.SaveChanges();

            var id = setup.context.Buyers.First().Id;
           
           var Buyer = await setup.buyerRepository.GetBuyerByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Buyers.First();            
                Assert.IsAssignableFrom<Buyer>(Buyer);
                Assert.IsAssignableFrom<Buyer>(result);
                Assert.Equal(Buyer.Id, result.Id);
            
            }
           
        }
  
    }
}
