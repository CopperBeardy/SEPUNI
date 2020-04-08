using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {

        public AddressRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Address>> GetAllAddressesAsync()
            => await FindAll().ToListAsync();

        public async Task<List<Address>> GetAllAddressesNotInUseAsync()
        {
            List<int> addressIds = _context.Listings.Select(x => x.AddressId).ToList();
            var address = await FindAll().ToListAsync();

            List<Address> response = new List<Address>();
            foreach (var addr in address)
            {
                if (!addressIds.Contains(addr.Id))
                {
                    response.Add(addr);
                }
            }
            return response;

        }

        public async Task<Address> GetAddressByIdAsync(int? id)
            => await FindByCondition(a => a.Id.Equals(id)).FirstOrDefaultAsync();

        public void CreateAddress(Address address)
            => Create(address);

        public void UpdateAddress(Address address)
            => Update(address);

        public bool Exists(int id)
            => _context.Addresses.Any(x => x.Id.Equals(id));

        public void DeleteAddress(Address address)
            => Delete(address);

    }
}
