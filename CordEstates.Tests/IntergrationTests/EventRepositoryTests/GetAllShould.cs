using CordEstates.Areas.Identity.Data;
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

namespace CordEstates.Tests.IntergrationTests.EventRepositoryTests
{
    public class GetAllShould 
    {
        DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();
        }
        [Fact]
        public async void ReturnListOfEvent()
        {
            foreach (Photo photo in setup.photos)
            { 
                setup.context.Add(photo);
            }             
            setup.context.SaveChanges();

           
            foreach (Event Event in setup.events)
            {
                
                setup.context.Events.Add(Event);
            }
              setup.context.SaveChanges();  
            
            var result = await setup.eventRepository.GetAllEventsAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(2, setup.contextConfirm.Events.Count());
                Assert.Equal(2, result.Count);
                Assert.IsAssignableFrom<List<Event>>(result);
            }
           
        }

      

        
    }
}
