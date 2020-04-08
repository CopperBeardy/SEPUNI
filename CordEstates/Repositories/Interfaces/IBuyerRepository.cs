using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IBuyerRepository : IRepositoryBase<Buyer>
    {
        bool Exists(int id);
        void DeleteBuyer(Buyer buyer);
        void UpdateBuyer(Buyer buyer);
        Task<Buyer> GetBuyerByIdAsync(int? id);
        void CreateBuyer(Buyer buyer);
        Task<List<Buyer>> GetAllBuyersAsync();
     


    }
}
