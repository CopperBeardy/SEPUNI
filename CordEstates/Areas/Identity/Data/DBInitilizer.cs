using CordEstates.Entities;
using System;

namespace CordEstates.Areas.Identity.Data
{

    public static class DbInitialize
    {
        public static void Initialize(ApplicationDbContext context)
        {

            //#region address
            //var address = new Address[]
            //{
            //    new Address
            //    {

            //        Number="22",
            //        FirstLine="My street",
            //        SecondLine="My village",
            //        TownCity="My city",
            //        Postcode="CF123TB"
            //    },
            //    new Address
            //    {

            //        Number="33",
            //        FirstLine="My street2",
            //        SecondLine="My village2",
            //        TownCity="My city2",
            //        Postcode="CF234TB"
            //    },
            //    new Address
            //    {

            //        Number="44",
            //        FirstLine="My street3",
            //        SecondLine="My village3",
            //        TownCity="My city3",
            //        Postcode="CF345TB"
            //    },
            //    new Address
            //    {

            //        Number="55",
            //        FirstLine="My street4",
            //        SecondLine="My village4",
            //        TownCity="My city4",
            //        Postcode="CF456TB"
            //    },
            //    new Address
            //    {

            //        Number="66",
            //        FirstLine="My street5",
            //        SecondLine="My village5",
            //        TownCity="My city5",
            //        Postcode="CF567TB"
            //    },
            //       new Address
            //    {

            //        Number="77",
            //        FirstLine="My street6",
            //        SecondLine="My village6",
            //        TownCity="My city6",
            //        Postcode="CF678TB"
            //    }
            //};

            //foreach (Address addr in address)
            //{
            //    context.Addresses.Add(addr);
            //}
            //context.SaveChanges();
            //#endregion

            //#region Ticket
            //var tickets = new Ticket[]
            //{
            //    new Ticket
            //    {
            //        FirstName = "Bob",
            //        LastName = "Brot",
            //        Email = "BobBrot@doo.com",
            //        Message = "This is the sample message for Bob Brot",
            //        Actioned = false,
            //        SentAt = DateTime.UtcNow
            //    },
            //    new Ticket
            //    {
            //        FirstName = "Sue",
            //        LastName = "Sally",
            //        Email = "SueSally@doo.com",
            //        Message = "This is the sample message for Sue Sally",
            //        Actioned = false,
            //        SentAt = DateTime.UtcNow.AddDays(1)
            //    },
            //    new Ticket
            //    {
            //        FirstName = "Tam",
            //        LastName = "Tombo",
            //        Email = "TamTombo@doo.com",
            //        Message = "This is the sample message for Tam Tombo",
            //        Actioned = false,
            //        SentAt = DateTime.UtcNow.AddDays(-1)
            //    },
            //};

            //foreach (Ticket ticket in tickets)
            //{
            //    context.Tickets.Add(ticket);
            //}

            //context.SaveChanges();
            //#endregion

            //#region Service

            //var services = new Service[]
            //{
            //    new Service { ServiceName="Service 1", Description="this is the description for the Service 1 for the project" },
            //    new Service { ServiceName="Service 2", Description="this is the description for the Service 2 for the project" },
            //    new Service { ServiceName="Service 3", Description="this is the description for the Service 3 for the project" }
            //};
            //foreach (Service service in services)
            //{
            //    context.Services.Add(service);
            //}
            //context.SaveChanges();

            //#endregion

            //#region Photo
            //var photos = new Photo[]
            //{
            //    new Photo {  ImageLink="images/ag1.jpg" },
            //    new Photo {  ImageLink="images/ag2.jpg" },
            //    new Photo {  ImageLink="images/ag3.jpg" },
            //    new Photo {  ImageLink="images/ag4.jpg" },
            //    new Photo {  ImageLink="images/ag5.jpg" },
            //    new Photo {  ImageLink="images/im1.jpg" },
            //    new Photo {  ImageLink="images/im2.jpg" },
            //    new Photo {  ImageLink="images/im3.jpg" },
            //    new Photo {  ImageLink="images/im4.jpg" },
            //    new Photo { ImageLink="images/im5.jpg" },
            //    new Photo {  ImageLink="images/im6.jpg" },
            //    new Photo {  ImageLink="images/ev1.jpg" },
            //    new Photo {  ImageLink="images/ev2.jpg" },

            //};

            //foreach (Photo photo in photos)
            //{
            //    context.Photos.Add(photo);
            //}
            //context.SaveChanges();
            //#endregion

            //#region Listing
            //var listings = new Listing[]
            //{
            //    new Listing {  AddressId= 1, ImageId= 6, Price=44000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //    new Listing {  AddressId= 2, ImageId= 7, Price=112000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //    new Listing {  AddressId= 3, ImageId= 8, Price=68000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //    new Listing {  AddressId= 4, ImageId= 9, Price=58000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //    new Listing {  AddressId= 5, ImageId= 10, Price=89000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //    new Listing {  AddressId= 6, ImageId= 11, Price=71000, Status= SaleStatus.Sold, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
            //};

            //foreach (Listing listing in listings)
            //{
            //    context.Listings.Add(listing);
            //}
            //context.SaveChanges();

            //#endregion

            //#region Event

            //var events = new Event[] 
            //{
            //    new Event{PhotoId=12, EventName="Open house", Details="open house will be at property balasdjfsdjf dsfsdf sdfsd fs dfsdf", Time=DateTime.UtcNow.AddDays(26), Active= true },
            //    new Event{PhotoId=13, EventName="Open house", Details="open house will be at property balasdjfsdjf dsfsdf sdfsd fs dfsdf", Time=DateTime.UtcNow.AddDays(-26), Active= false }
            //};
            //foreach (Event eve in events)
            //{
            //    context.Events.Add(eve);
            //}
            //context.SaveChanges();


            //#endregion

            #region Appointments

            var appointments = new Appointment[]
            {

                new Appointment {  ListingId=1,Time=DateTime.Now.AddDays(6),StaffId="9ad08ba8-3c2b-4a01-a384-884ea729dd77" },
                new Appointment { ListingId=2,Time=DateTime.Now.AddDays(3),StaffId="9ad08ba8-3c2b-4a01-a384-884ea729dd77" },
                new Appointment {  ListingId=2,Time=DateTime.Now.AddDays(2),StaffId="9ad08ba8-3c2b-4a01-a384-884ea729dd77" },
                new Appointment { ListingId=3,Time=DateTime.Now.AddDays(4),StaffId="9ad08ba8-3c2b-4a01-a384-884ea729dd77" },
                new Appointment { ListingId=1,Time=DateTime.Now.AddDays(2),StaffId="9ad08ba8-3c2b-4a01-a384-884ea729dd77" },
                new Appointment {  ListingId=2,Time=DateTime.Now.AddDays(5),StaffId="5da5aeb7-f198-442d-8998-0ec87604d07b" },
                new Appointment {  ListingId=4,Time=DateTime.Now.AddDays(2),StaffId="5da5aeb7-f198-442d-8998-0ec87604d07b" },
                new Appointment {  ListingId=4,Time=DateTime.Now.AddDays(4),StaffId="5da5aeb7-f198-442d-8998-0ec87604d07b" },
                new Appointment {  ListingId=5,Time=DateTime.Now.AddDays(5),StaffId="860d343d-c6a3-4667-add2-4023f2b89fc6" },
                new Appointment {  ListingId=1,Time=DateTime.Now.AddDays(2),StaffId="a8784218-d52c-4171-b755-0a78a92a3851" },
                new Appointment {  ListingId=3,Time=DateTime.Now.AddDays(8),StaffId="ebc291f8-be9f-46ab-968a-c5f99b747a41" },


            };

            foreach (Appointment appointment in appointments)
            {
                context.Appointments.Add(appointment);
            }
            context.SaveChanges();
            #endregion

        }
    }
}
