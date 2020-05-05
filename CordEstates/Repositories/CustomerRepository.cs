using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using CordEstates.Migrations;
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

        public async Task<Customer> GetCustomerByUserId(string userId ) =>
          await FindByCondition(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();


        public async Task<Customer> GetCustomerByIdAsync(int? id) =>
            await FindByCondition(x => x.Id.Equals(id)).Include(u => u.User).FirstOrDefaultAsync();

        public async Task<Customer> GetCustomersPropertyAsync(int? id) =>  
            await FindByCondition(x => x.Id.Equals(id)).Include(p => p.PropertiesInterestedIn).ThenInclude(l => l.Listing).ThenInclude(a => a.Address).FirstOrDefaultAsync();
          
        
       
        public async Task<List<Customer>> GetAllCustomersAsync() => await FindAll().Include(u => u.User).ToListAsync();
        public async Task<List<Customer>> GetAllCustomersPropertiesAsync() => 
            await FindAll().Include(p => p.PropertiesInterestedIn)
            .ThenInclude(l => l.Listing)
            .ThenInclude(a =>a.Address).ToListAsync();


        public void CreateCustomer(Customer eve) => Create(eve);

        public void UpdateCustomer(Customer eve) => Update(eve);

        public void DeleteCustomer(Customer CustomerItem) => Delete(CustomerItem);

        public bool Exists(int id) => Context.Customers.Any(x => x.Id.Equals(id));

        public async Task ToggleFollow(string user,Listing listing,bool follow)
        {

           var  cust =await GetCustomerByUserId(user);
            Customer customer = await GetCustomersPropertyAsync(cust.Id);

            if(follow)
            {
                if(customer.PropertiesInterestedIn == null)
                {
                    customer.PropertiesInterestedIn = new List<CustomerProperties>();
                }
                CustomerProperties customerProperties = new CustomerProperties { Customer = cust, Listing = listing };
                customer.PropertiesInterestedIn.Add(customerProperties);
            }else
            {
                CustomerProperties customerProperties = new CustomerProperties { Customer = cust, Listing = listing };

                customer.PropertiesInterestedIn.Remove(customerProperties);
            }
            
            UpdateCustomer(customer);
     
        }

    }
}
