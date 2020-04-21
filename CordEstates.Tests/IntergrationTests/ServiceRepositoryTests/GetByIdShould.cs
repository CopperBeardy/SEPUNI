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

namespace CordEstates.Tests.IntergrationTests.ServiceRepositoryTests
{
    public class GetByIdShould
    {
        readonly DatabaseSetup setup;
        public GetByIdShould()
        {
            setup = new DatabaseSetup();
        }

        [Fact]
        public async void GetServiceById()
        {
            var s = setup.services.First();
            setup.context.Add(s);

            setup.context.SaveChanges();

            var id = setup.context.Services.First().Id;

            var service = await setup.serviceRepository.GetServiceByIdAsync(id);

            using (setup.contextConfirm)
            {
                var result = setup.contextConfirm.Services.First();
                Assert.IsAssignableFrom<Service>(service);
                Assert.IsAssignableFrom<Service>(result);
                Assert.Equal(service.Id, result.Id);
            }

        }

    }
}
