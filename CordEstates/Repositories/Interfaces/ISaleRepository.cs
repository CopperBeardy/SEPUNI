using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface ISaleRepository : IRepositoryBase<Sale>
    {
        bool Exists(int id);
        void DeleteSale(Sale SaleItem);
        void UpdateSale(Sale eve);
        Task<Sale> GetSaleByIdAsync(int? id);
        void CreateSale(Sale eve);
        Task<List<Sale>> GetAllSalesAsync();
   


    }
}
