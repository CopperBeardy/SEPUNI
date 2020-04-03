using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Models.Enums;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class ListingRepository : RepositoryBase<Listing>, IListingRepository
    {

        public ListingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Listing>> GetAllListingsAsync()
            => await FindAll().Include(a => a.Address).Include(i => i.Image).ToListAsync();

        public async Task<List<Listing>> GetAllListingsForSaleAsync()
            => await FindAll()
            .Include(a => a.Address)
            .Include(p => p.Image)
            .Where(x => x.Status.Equals(SaleStatus.ForSale))
            .ToListAsync();

        public async Task<Listing> GetListingByIdAsync(int? id)
            => await FindByCondition(x => x.Id.Equals(id))
            .Include(a => a.Address)
            .Include(p => p.Image)
            .FirstOrDefaultAsync();

        public async Task<List<Listing>> GetLandingPageListingsAsync(int amount)
            => await FindAll().Take(amount).Include(x => x.Image).ToListAsync();
        public void CreateListing(Listing listing) => Create(listing);
        public void DeleteListing(Listing listing) => Delete(listing);
        public void UpdateListing(Listing listing) => Update(listing);
        public bool Exists(int id) => _context.Listings.Any(i => i.Id.Equals(id));


    }
}
