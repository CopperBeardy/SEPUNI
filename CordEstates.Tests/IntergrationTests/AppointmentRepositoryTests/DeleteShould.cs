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
    public class DeleteShould 
    {
        readonly DatabaseSetup setup;
        public DeleteShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public  void DeleteAppointment() 
        {
            var appointment = setup.appointments.First();
            using (setup.context)
            {
                //add Appointment to the db
                setup.appointmentRepository.CreateAppointment(appointment);
                setup.context.SaveChanges();
                // remove Appointment
                setup.appointmentRepository.DeleteAppointment(appointment);
                setup.context.SaveChanges();
            }

            

            using(setup.contextConfirm)
            {
                Assert.Equal(0, setup.contextConfirm.Appointments.Count());
           
            }
           
        }
  
    }
}
