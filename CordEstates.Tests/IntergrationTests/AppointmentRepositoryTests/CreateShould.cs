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
    public class CreateShould 
    {
        readonly DatabaseSetup setup;
        public CreateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void AddAppointment() 
        {
            var appointment = setup.appointments.First();
            using (setup.context)
            {

                setup.appointmentRepository.CreateAppointment(appointment);
                setup.context.SaveChanges();  
            }

            using(setup.contextConfirm)
            {
                Assert.Equal(1, setup.contextConfirm.Appointments.Count());
                var obj = setup.contextConfirm.Appointments.First();
                Assert.Equal(appointment.Time, obj.Time);
            }
           
        }
  
    }
}
