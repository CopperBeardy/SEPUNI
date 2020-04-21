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

namespace CordEstates.Tests.IntergrationTests.EventRepositoryTests
{
    public class DeleteShould 
    {
        DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteEvent() 
        {
            var Event = setup.events.First();
            using (setup.context)
            {
                //add Event to the db
                setup.eventRepository.CreateEvent(Event);
                setup.context.SaveChanges();
                // remove Event
                setup.eventRepository.DeleteEvent(Event);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Events.Count());
           
            }
           
        }
  
    }
}
