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

namespace CordEstates.Tests.IntergrationTests.TicketRepositoryTests
{
    public class UpdateShould 
    {
        DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateTicket() 
        {
           
            setup.context.Add(setup.tickets.First());
            setup.context.SaveChanges();

            var ticket = setup.context.Tickets.First();
           
            ticket.FirstName = "NewName";
            setup.ticketRepository.UpdateTicket(ticket);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Tickets.First();            
                
                Assert.Equal("NewName", result.FirstName);
            }
           
        }
  
    }
}
