using CordEstates.Areas.Identity.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CordEstates.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<ApplicationUser>
    {
   
        bool Exists(string id);
        void DeleteUser(ApplicationUser user);
        void UpdateUser(ApplicationUser user);
        void CreateUser(ApplicationUser user);
        Task<List<ApplicationUser>> GetAllStaffAsync();
        Task<ApplicationUser> GetStaffByIdAsync(string id);

        Task<string> GetUserId(ClaimsPrincipal claimsPrincipal);
    }
}
