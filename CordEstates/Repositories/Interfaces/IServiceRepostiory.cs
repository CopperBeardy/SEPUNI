using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        bool Exists(int id);
        void UpdateService(Service service);
        void CreateService(Service service);
        Task<Service> GetServiceByIdAsync(int? id);
        Task<List<Service>> GetAllServicesAsync();
        void DeleteService(Service service);
    }
}
