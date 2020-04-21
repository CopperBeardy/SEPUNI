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
    public class CreateShould 
    {
        DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddTicket() 
        {
            var ticket = setup.tickets.First();
            using (setup.context)
            {

                setup.ticketRepository.CreateTicket(ticket);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Tickets.Count());
                var obj = setup.contextConfirm.Tickets.First();
                Assert.Equal(ticket.FirstName, obj.FirstName);
            }
           
        }
  
    }
}
