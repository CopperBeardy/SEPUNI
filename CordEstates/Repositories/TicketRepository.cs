using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class TicketRepository : RepositoryBase<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
    {}
        public void CreateTicket(Ticket ticket) 
            => CreateTicket(ticket);
        public async Task<Ticket> GetTicketByIdAsync(string id)
            => await FindByCondition(i => i.Id.Equals(id)).FirstOrDefaultAsync();        

        public async Task<List<Ticket>> GetAllTicketsAsync() 
            => await FindAll().ToListAsync();
        
        public bool Exists(string id) 
            => _context.Tickets.Any(i => i.Id.Equals(id));

        public void UpdateTicket(Ticket ticketItem) 
            => Update(ticketItem);
        

    }
}
