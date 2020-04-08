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
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {

        readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> _userManager) : base(context)
        {
            this._userManager = _userManager;

        }
        public void CreateUser(ApplicationUser user) => CreateUser(user);



        public async Task<ApplicationUser> GetStaffByIdAsync(string id)
            => await FindByCondition(x => x.Id.Equals(id)).Include(h => h.HeadShot).FirstOrDefaultAsync();


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

        public void UpdateUser(ApplicationUser user) => Update(user);

        public void DeleteUser(ApplicationUser user) => Delete(user);

        public bool Exists(string id)
            => _context.Users.Any(i => i.Id.Equals(id));
        public async Task<string> GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            ApplicationUser user = await _userManager.GetUserAsync(claimsPrincipal);

            return user.Id;
        }
    }
}
