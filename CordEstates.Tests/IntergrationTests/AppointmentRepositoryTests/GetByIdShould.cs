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

namespace CordEstates.Tests.IntergrationTests.AppointmentRepositoryTests
{
    public class GetByIdShould 
    {
        readonly DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();  
            //create address
            var add = setup.addresses.First();
            setup.context.Addresses.Add(add);
            setup.context.SaveChanges();
            
            //create listing and associate with address
            var listing = setup.listings.First();
            listing.AddressId = add.Id;
            setup.context.Listings.Add(listing);
            setup.context.SaveChanges();

            //create User
            ApplicationUser user = new ApplicationUser() { Id = "test user" };
            setup.context.Users.Add(user);

            //create appointment and associate with address & user
            var a = setup.appointments.First();
            a.ListingId = listing.Id;
            a.StaffId = user.Id;
            setup.context.Add(a);
        }

        [Fact]
        public async void GetAppointmentById() 
        {   
            setup.context.SaveChanges();

            var id = setup.context.Appointments.First().Id;
           
           var appointment = await setup.appointmentRepository.GetAppointmentByIdAsync(id);
            
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Appointments.First();            
                Assert.IsAssignableFrom<Appointment>(appointment);
                Assert.IsAssignableFrom<Appointment>(result);
                Assert.Equal(appointment.Id, result.Id);
                Assert.Equal(appointment.ListingId, result.ListingId);
                Assert.Equal(appointment.StaffId, result.StaffId);

            }
           
        }
  
    }
}
