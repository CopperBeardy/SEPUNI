using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class BuyerRepository : RepositoryBase<Buyer>, IBuyerRepository
    {


        public BuyerRepository(ApplicationDbContext context) : base(context)
        {
        }


        

        public async Task<Buyer> GetBuyerByIdAsync(int? id) =>
            await FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();


        public async Task<List<Buyer>> GetAllBuyersAsync() => await FindAll().ToListAsync();


        public void CreateBuyer(Buyer eve) => Create(eve);

        public void UpdateBuyer(Buyer eve) => Update(eve);

        public void DeleteBuyer(Buyer BuyerItem) => Delete(BuyerItem);

        public bool Exists(int id) => _context.Buyers.Any(x => x.Id.Equals(id));


    }
}
