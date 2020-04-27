using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {


        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }


        

        public async Task<Customer> GetCustomerByIdAsync(int? id) =>
            await FindByCondition(x => x.Id.Equals(id)).Include(u => u.User).FirstOrDefaultAsync();


        public async Task<List<Customer>> GetAllCustomersAsync() => await FindAll().Include(u => u.User).ToListAsync();


        public void CreateCustomer(Customer eve) => Create(eve);

        public void UpdateCustomer(Customer eve) => Update(eve);

        public void DeleteCustomer(Customer CustomerItem) => Delete(CustomerItem);

        public bool Exists(int id) => _context.Customers.Any(x => x.Id.Equals(id));

      

        
    }
}
