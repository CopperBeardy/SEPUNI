using AutoMapper;
using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Helpers;
using CordEstates.Wrappers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace CordEstates.Tests.SetupFixtures
{
    public class SetupFixture
    {
        public Mock<ILoggerManager> Logger;
        public Mock<IMapper> mapper;
        public Mock<IRepositoryWrapper> repositoryWrapper;
        public Mock<ApplicationDbContext> context;


        public SetupFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb").Options;
            Logger = new Mock<ILoggerManager>();
            mapper = new Mock<IMapper>();
            context = new Mock<ApplicationDbContext>(options);
            repositoryWrapper = new Mock<IRepositoryWrapper>();
        }

        public void SetupContext()
        {
            context.Setup(_ => _.Set<Event>()).Returns(new Mock<DbSet<Event>>().Object);
            context.Setup(_ => _.Set<Appointment>()).Returns(new Mock<DbSet<Appointment>>().Object);
            context.Setup(_ => _.Set<Address>()).Returns(new Mock<DbSet<Address>>().Object);
            context.Setup(_ => _.Set<Listing>()).Returns(new Mock<DbSet<Listing>>().Object);
            context.Setup(_ => _.Set<Photo>()).Returns(new Mock<DbSet<Photo>>().Object);
            context.Setup(_ => _.Set<Service>()).Returns(new Mock<DbSet<Service>>().Object);
            context.Setup(_ => _.Set<Ticket>()).Returns(new Mock<DbSet<Ticket>>().Object);
            context.Setup(_ => _.Set<ApplicationUser>()).Returns(new Mock<DbSet<ApplicationUser>>().Object);

        }




    }
}
