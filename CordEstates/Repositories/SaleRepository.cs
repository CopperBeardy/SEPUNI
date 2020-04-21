using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
    {


        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }


     
        public async Task<Sale> GetSaleByIdAsync(int? id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .Include(b => b.Buyer)
            .Include(s => s.SellingAgent)
            .Include(p => p.SoldProperty)
            .FirstOrDefaultAsync();


        public async Task<List<Sale>> GetAllSalesAsync() => await FindAll()
            .Include(b => b.Buyer)
            .Include(s => s.SellingAgent)
            .ToListAsync();


        public void CreateSale(Sale eve) => Create(eve);

        public void UpdateSale(Sale eve) => Update(eve);

        public void DeleteSale(Sale SaleItem) => Delete(SaleItem);

        public bool Exists(int id) => _context.Sales.Any(x => x.Id.Equals(id));

      
    }
}
