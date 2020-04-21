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

namespace CordEstates.Tests.IntergrationTests.EventRepositoryTests
{
    public class UpdateShould 
    {
        readonly DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateEvent() 
        {
           
            setup.context.Add(setup.events.First());
            setup.context.SaveChanges();

            var Event = setup.context.Events.First();

            Event.EventName = "Change of text";
            setup.eventRepository.UpdateEvent(Event);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Events.First();            
                
                Assert.Equal("Change of text", result.EventName);
            }
           
        }
  
    }
}
