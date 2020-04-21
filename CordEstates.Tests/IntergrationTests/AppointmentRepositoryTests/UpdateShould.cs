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

namespace CordEstates.Tests.IntergrationTests.AppointmentRepositoryTests
{
    public class UpdateShould 
    {
        readonly DatabaseSetup setup;
        public UpdateShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public void UpdateAppointment() 
        {
           
            setup.context.Add(setup.appointments.First());
            setup.context.SaveChanges();

            var Appointment = setup.context.Appointments.First();
            DateTime dateTime =  DateTime.Now.AddDays(1);
            Appointment.Time = dateTime;
            setup.appointmentRepository.UpdateAppointment(Appointment);
            setup.context.SaveChanges();
        

            using(setup.contextConfirm)
            {
                var result = setup.contextConfirm.Appointments.First();            
                
                Assert.Equal(dateTime, result.Time);
            }
           
        }
  
    }
}
