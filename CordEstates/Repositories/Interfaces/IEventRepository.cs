using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
        bool Exists(int id);
        void DeleteEvent(Event eventItem);
        void UpdateEvent(Event eve);
        Task<Event> GetEventByIdAsync(int? id);
        void CreateEvent(Event eve);
        Task<List<Event>> GetAllEventsAsync();
        Task<Event> GetActiveEventAsync();


    }
}
