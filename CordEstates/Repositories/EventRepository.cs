using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class EventRepository : RepositoryBase<Event> ,IEventRepository
    {
     

        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<Event> GetActiveEventAsync() => await FindByCondition(x => x.Active.Equals(true)).Include(y => y.Photo).FirstOrDefaultAsync();


        public async Task<Event> GetEventByIdAsync(int? id) => 
            await FindByCondition(x => x.Id.Equals(id)).Include(y => y.Photo).FirstOrDefaultAsync();
        

        public async Task<List<Event>> GetAllEventsAsync() => await FindAll().Include(x => x.Photo).ToListAsync();


        public void CreateEvent(Event eve) => Create(eve);

        public void UpdateEvent(Event eve) => Update(eve);

        public void DeleteEvent(Event eventItem) => Delete(eventItem);

        public bool Exists(int id)=>  _context.Events.Any(x => x.Id.Equals(id));

       
    }
}
