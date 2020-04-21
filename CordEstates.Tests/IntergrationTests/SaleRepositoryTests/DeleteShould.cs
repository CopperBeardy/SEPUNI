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
    public class DeleteShould 
    {
        readonly DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteSale() 
        {
            var Sale = setup.sales.First();
            using (setup.context)
            {
                //add Sale to the db
                setup.saleRepository.CreateSale(Sale);
                setup.context.SaveChanges();
                // remove Sale
                setup.saleRepository.DeleteSale(Sale);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Sales.Count());
           
            }
           
        }
  
    }
}
