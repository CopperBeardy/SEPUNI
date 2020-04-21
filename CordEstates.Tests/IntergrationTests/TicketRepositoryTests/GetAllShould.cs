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

namespace CordEstates.Tests.IntergrationTests.TicketRepositoryTests
{
    public class GetAllShould 
    {
        DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfTickets()
        {

            foreach (Ticket ticket in setup.tickets)
            {
                setup.context.Tickets.Add(ticket);
            }
              setup.context.SaveChanges();  
            
            var result = await setup.ticketRepository.GetAllTicketsAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Tickets.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Ticket>>(result);
            }
           
        }
    }
}
