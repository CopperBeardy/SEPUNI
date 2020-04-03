using CordEstates.Helpers;
using CordEstates.Repositories;
using CordEstates.Repositories.Interfaces;
using CordEstates.Services;
using CordEstates.Wrappers;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CordEstates.Helpers.ImageUpload;

namespace CordEstates.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static void RepositoryServices(IServiceCollection services)
        {
            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<IListingRepository, ListingRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IImageUploadWrapper, ImageUploadWrapper>();

        }
    }
}
