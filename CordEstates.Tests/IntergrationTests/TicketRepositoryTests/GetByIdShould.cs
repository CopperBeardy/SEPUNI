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

namespace CordEstates.Tests.IntergrationTests.TicketRepositoryTests
{
    public class GetByIdShould 
    {
        DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetTicketById() 
        {
           
            setup.context.Add(setup.tickets.First());
            setup.context.SaveChanges();

            var id = setup.context.Tickets.First().Id;
           
           var ticket = await setup.ticketRepository.GetTicketByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Tickets.First();            
                Assert.IsAssignableFrom<Ticket>(ticket);
                Assert.IsAssignableFrom<Ticket>(result);
                Assert.Equal(ticket.Id, result.Id);
            
            }
           
        }
  
    }
}
