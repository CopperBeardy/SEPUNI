using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class AppointmentManagementDTO
    {

        public int Id { get; set; }

        public ApplicationUser Staff { get; set; }

        public Listing Listing { get; set; }

        public DateTime Time { get; set; }

      


    }
}
