using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface ITicketRepository : IRepositoryBase<Ticket>
    {
        void CreateTicket(Ticket Ticket);

        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(string id);
        bool Exists(string id);
        void UpdateTicket(Ticket ticketItem);

    }
}
