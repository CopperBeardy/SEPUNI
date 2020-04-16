using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories;
using CordEstates.Tests.Setup;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace CordEstates.Tests.IntergrationTests.AppointmentRepositoryTests
{
    public class GetAllShould 
    {
        readonly DatabaseSetup setup;
        public GetAllShould()
        {
            setup = new DatabaseSetup();

            //create address
            foreach (Address address in setup.addresses)
            {  
                setup.context.Addresses.Add(address);
            }
         setup.context.SaveChanges();

            var addresses = setup.context.Addresses.ToList();  
            //create listing and associate with address
            for (int i = 0; i < 3; i++)
            {  
                var listing = setup.listings[i];
                listing.AddressId = addresses[i].Id;
                setup.context.Listings.Add(listing);

            }          
            setup.context.SaveChanges();

            //create User
            ApplicationUser user = new ApplicationUser() { Id = "test user" };
            ApplicationUser user2 = new ApplicationUser() { Id = "test user2" };                       
            setup.context.Users.Add(user);
            setup.context.Users.Add(user2);
            setup.context.SaveChanges();

            //create appointment and associate with address & user
            var users = setup.context.Users.ToList();
            var listings = setup.context.Listings.ToList();
            for (int i = 0; i < 3; i++)
            {
                var a = setup.appointments[i]; 
                 a.ListingId = listings[i].Id;
                if(i == 0)
                {
                    a.StaffId = users[0].Id;
                }
                else
                {
                    a.StaffId = users[1].Id;
                }

                 setup.context.Add(a);

            }
            setup.context.SaveChanges();   
        }
        [Fact]
        public async void ReturnListOfAppointment()
        {      
            var result = await setup.appointmentRepository.GetAllAppointmentsAsync();

            using (setup.contextConfirm)
            {
                
                Assert.Equal(3, setup.contextConfirm.Appointments.Count());
                Assert.Equal(3, result.Count);
                Assert.IsAssignableFrom<List<Appointment>>(result);
            }
           
        }

        [Fact]
        public async void ReturnListOfAppointmentWhereAppointmentsMatchStaffId()
        {
            var result = await setup.appointmentRepository.GetAllAppointmentsByStaffIdAsync("test user");

            using (setup.contextConfirm)
            {

                Assert.Equal(3, setup.contextConfirm.Appointments.Count());
                Assert.Equal(2, result.Count);
                Assert.IsAssignableFrom<List<Appointment>>(result);
            }

        }
    }
}
