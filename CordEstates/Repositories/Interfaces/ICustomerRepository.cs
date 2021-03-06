﻿using CordEstates.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer> GetCustomerByUserId(string userId);
        Task<Customer> GetCustomersPropertyAsync(int? id);
        
        Task<List<Customer>> GetAllCustomersPropertiesAsync();
        
        bool Exists(int id);
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int? id);
        void CreateCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomersAsync();

        Task ToggleFollow(string user, Listing listing,bool follow);

    }
}
