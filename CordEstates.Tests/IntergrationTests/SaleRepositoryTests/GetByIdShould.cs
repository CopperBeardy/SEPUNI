using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Tests.Setup;
using System;
using System.Linq;
using Xunit;

namespace CordEstates.Tests.IntergrationTests.SaleRepositoryTests
{
    public class GetByIdShould
    {
        readonly DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetSaleById()
        {
            //add  buyer
            var buyer = setup.buyers.First();
            setup.context.Buyers.Add(buyer);
            setup.context.SaveChanges();

            //add user
            ApplicationUser user = new ApplicationUser() { Id = "Test user" };
            setup.context.Users.Add(user);
            setup.context.SaveChanges();

            //add address
            var address = setup.addresses.First();
            setup.context.Addresses.Add(address);
            setup.context.SaveChanges();

            //add sale and associate with buyer,user and address
            var sale = setup.sales.First();
            sale.AgentId = setup.context.Users.First().Id;
            sale.PropertyId = setup.context.Addresses.First().Id;
            sale.BuyerId = setup.context.Buyers.First().Id;
            setup.context.Add(sale);
            setup.context.SaveChanges();

            var id = setup.context.Sales.First().Id;

            var Sale = await setup.saleRepository.GetSaleByIdAsync(id);

            using (setup.contextConfirm)
            {
                var result = setup.contextConfirm.Sales.First();
                Assert.IsAssignableFrom<Sale>(Sale);
                Assert.IsAssignableFrom<Sale>(result);
                Assert.Equal(Sale.Id, result.Id);
                Assert.Equal(Sale.BuyerId, result.BuyerId);
                Assert.Equal(Sale.PropertyId, result.PropertyId);
                Assert.Equal(Sale.AgentId, result.AgentId);

            }

        }

    }
}
