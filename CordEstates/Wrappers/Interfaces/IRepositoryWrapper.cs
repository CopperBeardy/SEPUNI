using CordEstates.Repositories.Interfaces;
using System.Threading.Tasks;

namespace CordEstates.Wrappers.Interfaces
{
    public interface IRepositoryWrapper
    {
        ITicketRepository Ticket { get; }

        IListingRepository Listing { get; }

        IServiceRepository Service { get; }
        IEventRepository Event { get; }
        IUserRepository User { get; }
        IAppointmentRepository Appointment { get; }

        IAddressRepository Address { get; }
        IPhotoRepository Photo { get; }
        Task SaveAsync();
    }
}
