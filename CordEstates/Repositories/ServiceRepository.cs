using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {


        }

        public async Task<List<Service>> GetAllServicesAsync() => await FindAll().ToListAsync();


        public async Task<Service> GetServiceByIdAsync(int? id) => await FindByCondition(s => s.Id.Equals(id))
            .FirstOrDefaultAsync();


        public void CreateService(Service service) => Create(service);

        public void UpdateService(Service service) => Update(service);

        public void DeleteService(Service service) => Delete(service);

        public bool Exists(int id)
         => Context.Services.Any(i => i.Id.Equals(id));
    }
}
