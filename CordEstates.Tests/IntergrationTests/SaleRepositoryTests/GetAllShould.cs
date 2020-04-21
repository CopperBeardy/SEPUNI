using CordEstates.Areas.Identity.Data;
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

namespace CordEstates.Tests.IntergrationTests.SaleRepositoryTests
{
    public class GetAllShould 
    {
        readonly DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfSales()
        {
            foreach (Buyer buyer in setup.buyers)
            { 
                setup.context.Buyers.Add(buyer);
            } 
            setup.context.SaveChanges();

            foreach (ApplicationUser user in setup.users)
            {
                setup.context.Users.Add(user);
            }       
            setup.context.SaveChanges();

            foreach (Address address in setup.addresses)
            { 
                setup.context.Addresses.Add(address);
            }  
            setup.context.SaveChanges();
           
            var buyers = setup.context.Buyers.ToList();
            var addresses = setup.context.Addresses.ToList();
            var users = setup.context.Users.ToList();
            for(int i =0; i < 3;i++)
            {
                var sale = setup.sales[i];
                sale.AgentId = users[i].Id;
                sale.PropertyId = addresses[i].Id;
                sale.BuyerId = buyers[i].Id;
                setup.context.Add(sale);
            }  
            setup.context.SaveChanges();
            
            var result = await setup.saleRepository.GetAllSalesAsync();

            using (setup.contextConfirm)
            {                
                Assert.Equal(3, setup.contextConfirm.Sales.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Sale>>(result);
            }
           
        }

      

        
    }
}
