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

        IEmployeeRepository Employee { get; }

        IAppointmentRepository Appointment { get; }

        IAddressRepository Address { get; }

        IPhotoRepository Photo { get; }

        IBuyerRepository Buyer{ get;   }
        ISaleRepository Sale { get; }
        ICustomerRepository Customer { get; }
        Task SaveAsync();
    }
}
