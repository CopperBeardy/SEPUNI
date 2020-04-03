using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        Task<Address> GetAddressByIdAsync(int? id);
        Task<List<Address>> GetAllAddressesAsync();
        void CreateAddress(Address address);
        void UpdateAddress(Address address);
        bool Exists(int _id);
        void DeleteAddress(Address _address);
    }
}
