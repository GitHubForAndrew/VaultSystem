using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VaultSystem.Models;

namespace VaultSystem.Domain.Concrete
{
    public class AppUserRepository : Abstract.IUserRepository
    {
        private UserManager<ApplicationUser> userManager; 
        private RoleManager<IdentityRole> roleManager;

        public AppUserRepository()
        {
            ApplicationDbContext appContext = ApplicationDbContext.Create();
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appContext));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appContext));
        }

        public List<ApplicationUser> GetUsers()
        {
            return userManager.Users.ToList();
        }

        public List<IdentityRole> GetRoles()
        {
            return roleManager.Roles.ToList();
        }

        public bool AddUser(ApplicationUser user)
        {
            return userManager.Create(user).Succeeded;
        }

        public void DeleteUser(ApplicationUser user)
        {
            userManager.Delete(user);
        }

        public void AddToRole(ApplicationUser user, IdentityRole role)
        {
            userManager.AddToRole(user.Id, role.Name);
        }

        public void RemoveFromRole(ApplicationUser user, IdentityRole role)
        {
            userManager.RemoveFromRole(user.Id, role.Name);
        }

        public IList<string> GetUserRoles(ApplicationUser user)
        {
            return userManager.GetRoles(user.Id);
        }
    }
}
