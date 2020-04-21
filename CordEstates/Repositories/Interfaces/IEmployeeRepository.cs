using CordEstates.Areas.Identity.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepositoryBase<ApplicationUser>
    {
   
        Task<ApplicationUser> GetUserByIdAsync(string id);
     
        Task<List<ApplicationUser>> GetAllUsers();
        bool Exists(string id);
        void DeleteUser(ApplicationUser user);
        //Task UpdateUser(ApplicationUser user);
   
        Task<List<ApplicationUser>> GetAllStaffAsync();
        Task<ApplicationUser> GetStaffByIdAsync(string id);

        Task<string> GetUserId(ClaimsPrincipal claimsPrincipal);
    }
}
