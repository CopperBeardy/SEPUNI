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

namespace CordEstates.Tests.IntergrationTests.SaleRepositoryTests
{
    public class UpdateShould 
    {
        readonly DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateSale() 
        {
           
            setup.context.Add(setup.sales.First());
            setup.context.SaveChanges();

            var Sale = setup.context.Sales.First();

            Sale.AgreedPrice = 65656;
            setup.saleRepository.UpdateSale(Sale);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Sales.First();            
                
                Assert.Equal(65656, result.AgreedPrice);
            }
           
        }
  
    }
}
