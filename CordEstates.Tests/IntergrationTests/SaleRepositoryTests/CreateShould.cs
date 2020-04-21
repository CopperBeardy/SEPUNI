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

namespace CordEstates.Tests.IntergrationTests.SaleRepositoryTests
{
    public class CreateShould 
    {
        readonly DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddSale() 
        {
            var sale = setup.sales.First();
            using (setup.context)
            {

                setup.saleRepository.CreateSale(sale);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Sales.Count());
                var obj = setup.contextConfirm.Sales.First();
                Assert.Equal(sale.AgreedPrice, obj.AgreedPrice);
            }
           
        }
  
    }
}
