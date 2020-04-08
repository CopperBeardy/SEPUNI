using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IListingRepository : IRepositoryBase<Listing>
    {
        Task<List<Listing>> GetAllListingsAsync();
        Task<List<Listing>> GetAllListingsForSaleAsync();

        Task<List<Listing>> GetLandingPageListingsAsync(int amount);
        Task<Listing> GetListingByIdAsync(int? id);
        void CreateListing(Listing listing);
        void DeleteListing(Listing listing);
        void UpdateListing(Listing listing);
        bool Exists(int id);
    }
}
