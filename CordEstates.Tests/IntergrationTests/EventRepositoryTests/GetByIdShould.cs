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
    public class GetByIdShould 
    {
        DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetEventById() 
        {
            var photo = setup.photos.First();
            setup.context.Add(photo);
            setup.context.SaveChanges();
            var eve = setup.events.First();
            eve.PhotoId = setup.context.Photos.First().Id;
            setup.context.Add(eve);


            setup.context.SaveChanges();

            var id = setup.context.Events.First().Id;
           
           var Event = await setup.eventRepository.GetEventByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Events.First();            
                Assert.IsAssignableFrom<Event>(Event);
                Assert.IsAssignableFrom<Event>(result);
                Assert.Equal(Event.Id, result.Id);
            
            }
           
        }
  
    }
}
