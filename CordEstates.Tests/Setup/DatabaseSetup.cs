using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Models.Enums;
using CordEstates.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CordEstates.Tests.Setup
{

    public class DatabaseSetup : IDisposable
    {
        public ApplicationDbContext context;
        public ApplicationDbContext contextConfirm;


        public TicketRepository ticketRepository;
        public AddressRepository addressRepository;
        public AppointmentRepository appointmentRepository;
        public EventRepository eventRepository;
        public ListingRepository listingRepository;
        public ServiceRepository serviceRepository;
        public UserRepository userRepository;
        public PhotoRepository photoRepository;
        public BuyerRepository buyerRepository;
        public SaleRepository saleRepository;


        public List<Address> addresses;
        public List<Appointment> appointments;
        public List<Buyer> buyers;
        public List<Event> events;
        public List<Listing> listings;
        public List<Photo> photos;
        public List<Sale> sales;
        public List<Service> services;
        public List<Ticket> tickets;


        public DatabaseSetup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}").Options;

            context = new ApplicationDbContext(options);
            contextConfirm = new ApplicationDbContext(options);


            SetupDataObjects();
            SetupRepositories();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            ((IDisposable)context).Dispose();
            contextConfirm.Database.EnsureDeleted();
            ((IDisposable)contextConfirm).Dispose();
        }

        public void SetupDataObjects()
        {
            addresses = new List<Address>() { 
                new Address
                {
                    Id=1,
                    Number="22",
                    FirstLine="My street",
                    SecondLine="My village",
                    TownCity="My city",
                    Postcode="CF123TB"
                },
                new Address
                {Id =2,
                    Number="33",
                    FirstLine="My street2",
                    SecondLine="My village2",
                    TownCity="My city2",
                    Postcode="CF234TB"
                },
                new Address
                {Id=3,
                    Number="44",
                    FirstLine="My street3",
                    SecondLine="My village3",
                    TownCity="My city3",
                    Postcode="CF345TB"
                } };

          
            appointments = new List<Appointment>()
            {
                new Appointment
                { ListingId = 1 , Time = DateTime.Now.AddDays(6), StaffId = "1a93906b-69d9-435f-af15-68b3ca347ba7" },
                new Appointment
                { ListingId = 2, Time = DateTime.Now.AddDays(3), StaffId = "1a93906b-69d9-435f-af15-68b3ca347ba7" },
                new Appointment
                { ListingId = 3, Time = DateTime.Now.AddDays(2), StaffId = "1a93906b-69d9-435f-af15-68b3ca347ba7" }
            };

            buyers = new List<Buyer>()
            {
                new Buyer
                {
                    Id = 1,
                    Title = "Mr",
                    HouseNumber = 44,
                    FirstLine = "first one",
                    FirstName = "Person 1",
                    LastName = "one surname",
                    Postcode = "CF477DD",
                    PhoneNumber = "01443588462"
                },
                new Buyer
                {
                    Id = 2,
                    Title = "Ms",
                    HouseNumber = 24,
                    FirstLine = "second one",
                    FirstName = "Person 2",
                    LastName = "two surname",
                    Postcode = "DD984DD",
                    PhoneNumber = "01443534232"
                },
                new Buyer
                {
                    Id = 3,
                    Title = "Mrs",
                    HouseNumber = 11,
                    FirstLine = "third one",
                    FirstName = "Person 3",
                    LastName = "three surname",
                    Postcode = "EE246DD",
                    PhoneNumber = "01443233333"
                }
            };

            photos = new List<Photo>() {
                new Photo { Id=1,  ImageLink="images/im1.jpg" },
                new Photo { Id=2, ImageLink="images/im2.jpg" },
                new Photo { Id=3, ImageLink="images/im3.jpg" },
            };

            listings = new List<Listing>() {
                new Listing { Id=1,  AddressId= 1, ImageId= 1, Price=44000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
                new Listing { Id=2, AddressId= 2, ImageId= 2, Price=112000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
                new Listing { Id=3, AddressId= 3, ImageId= 3, Price=68000, Status= SaleStatus.ForSale, Description="blah blskdf sdf sdf sdf sdfwef fwejfegu sdfmsdf msdfm sdfo sdf " },
               };

            

events = new List<Event>()
            {
                new Event{Id=1,PhotoId=1, EventName="Open house", Details="open house will be at property balasdjfsdjf dsfsdf sdfsd fs dfsdf", Time=DateTime.UtcNow.AddDays(26), Active= true },
                new Event{Id=2, PhotoId=2, EventName="Open house", Details="open house will be at property balasdjfsdjf dsfsdf sdfsd fs dfsdf", Time=DateTime.UtcNow.AddDays(-26), Active= false }
            };
            tickets = new List<Ticket>()
            {
                new Ticket()
                { Id="1",FirstName = "Test", LastName = "bob", Email = "d@D.com", Message = "Test" },
                new Ticket()
                {Id="2", FirstName = "Test3", LastName = "bob2", Email = "d@D1.com", Message = "Test2" },
                new Ticket()
                { Id="3",FirstName = "Test3", LastName = "bob3", Email = "d@D2.com", Message = "Test3" },
            };

            sales = new List<Sale>() { 
                new Sale {Id=1, BuyerId=1 ,PropertyId=1, AgreedPrice=10000, DateOfSale=DateTime.Now, AgentId= Guid.NewGuid().ToString()  },
                new Sale {Id=2, BuyerId=2 ,PropertyId=2, AgreedPrice=20000, DateOfSale=DateTime.Now.AddDays(-1), AgentId= Guid.NewGuid().ToString()  },
                new Sale {Id=3, BuyerId=3 ,PropertyId=3, AgreedPrice=30000, DateOfSale=DateTime.Now.AddDays(-2), AgentId= Guid.NewGuid().ToString()  },

            };


            services = new List<Service>()  {
                new Service {Id=1, ServiceName="Service 1", Description="this is the description for the Service 1 for the project" },
                new Service {Id=2, ServiceName="Service 2", Description="this is the description for the Service 2 for the project" },
                new Service {Id=3, ServiceName="Service 3", Description="this is the description for the Service 3 for the project" }
            };

           


        }

        public void SetupRepositories()
        {

            addressRepository = new AddressRepository(context);
            appointmentRepository = new AppointmentRepository(context);
            buyerRepository = new BuyerRepository(context);
            eventRepository = new EventRepository(context);
            listingRepository = new ListingRepository(context);
            photoRepository = new PhotoRepository(context);
            saleRepository = new SaleRepository(context);
            serviceRepository = new ServiceRepository(context);
            ticketRepository = new TicketRepository(context);


        }

    }
}
