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
using Xunit;
using Xunit.Sdk;

namespace CordEstates.Tests.IntergrationTests.EventRepositoryTests
{
    public class CreateShould 
    {
        DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddEvent() 
        {
            var Event = setup.events.First();
            using (setup.context)
            {

                setup.eventRepository.CreateEvent(Event);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Events.Count());
                var obj = setup.contextConfirm.Events.First();
                Assert.Equal(Event.EventName, obj.EventName);
            }
           
        }
  
    }
}
