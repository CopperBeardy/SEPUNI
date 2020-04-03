using CordEstates.Areas.Identity.Data;
using CordEstates.Repositories;
using CordEstates.Repositories.Interfaces;
using CordEstates.Wrappers.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Wrappers
{
    public class RepositoryWrapper : IRepositoryWrapper
    {


        private AddressRepository _address;
        private AppointmentRepository _appointment;
        private EventRepository _event;
        private ListingRepository _listing;
        private ServiceRepository _service;
        private TicketRepository _ticket;
        private UserRepository _user;
        private PhotoRepository _photo;



        readonly ApplicationDbContext _context;
        readonly UserManager<ApplicationUser> _userManager;
        public RepositoryWrapper(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ITicketRepository Ticket
        {
            get
            {
                if (_ticket == null)
                {
                    _ticket = new TicketRepository(_context);
                }

                return _ticket;
            }
        }


        public IListingRepository Listing
        {
            get
            {
                if (_listing == null)
                {
                    _listing = new ListingRepository(_context);
                }

                return _listing;
            }
        }
        public IServiceRepository Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ServiceRepository(_context);
                }

                return _service;
            }
        }

        public IEventRepository Event
        {
            get
            {
                if (_event == null)
                {
                    _event = new EventRepository(_context);
                }

                return _event;
            }
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context, _userManager);
                }

                return _user;
            }
        }
        public IAppointmentRepository Appointment
        {
            get
            {
                if (_appointment == null)
                {
                    _appointment = new AppointmentRepository(_context);
                }

                return _appointment;
            }
        }
        public IAddressRepository Address
        {
            get
            {
                if (_address == null)
                {
                    _address = new AddressRepository(_context);
                }

                return _address;
            }
        }
        public IPhotoRepository Photo
        {
            get
            {
                if (_photo == null)
                {
                    _photo = new PhotoRepository(_context);
                }

                return _photo;
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
