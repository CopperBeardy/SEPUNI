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

namespace CordEstates.Tests.IntergrationTests.BuyerRepositoryTests
{
    public class GetAllShould 
    {
        DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfBuyer()
        {

            foreach (Buyer Buyer in setup.buyers)
            {
                setup.context.Buyers.Add(Buyer);
            }
              setup.context.SaveChanges();  
            
            var result = await setup.buyerRepository.GetAllBuyersAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Buyers.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Buyer>>(result);
            }
           
        }

    

        
    }
}
