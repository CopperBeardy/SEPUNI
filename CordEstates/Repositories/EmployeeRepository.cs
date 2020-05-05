using CordEstates.Areas.Identity.Data;
using CordEstates.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CordEstates.Repositories
{
    public class EmployeeRepository : RepositoryBase<ApplicationUser>, IEmployeeRepository
    {

        readonly UserManager<ApplicationUser> _userManager;

        public EmployeeRepository(ApplicationDbContext context,
            UserManager<ApplicationUser> _userManager) : base(context)
        {
           
            this._userManager = _userManager;
        }
      

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
         => await FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();


        public async Task<ApplicationUser> GetStaffByIdAsync(string id)
            => await FindByCondition(x => x.Id.Equals(id)).Include(h => h.HeadShot).FirstOrDefaultAsync();

        public async Task<List<ApplicationUser>> GetAllUsers() => await FindAll().ToListAsync();
             
         public async Task<List<ApplicationUser>> GetAllStaffAsync()
        {

            IList<ApplicationUser> users = await _userManager.GetUsersInRoleAsync("Staff");
            List<ApplicationUser> agents = new List<ApplicationUser>();
            foreach (var item in users)
            {
                agents.Add(await FindAll().Include(x => x.HeadShot).FirstOrDefaultAsync(x => x.Id.Equals(item.Id)));
            }


            //_context.Users.Where(a => users.Any(c => c.Equals(a.Id))).Include(c => c.HeadShot).ToList();
            return agents;
        }

        //public async Task UpdateUser(ApplicationUser user)
        //{ 
        //    await _userManager.UpdateAsync(user);
        //}


        public void DeleteUser(ApplicationUser user) => Delete(user);

        public bool Exists(string id)
            => Context.Users.Any(i => i.Id.Equals(id));
        public async Task<string> GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(claimsPrincipal);

            return user.Id;
        }


    }
}
