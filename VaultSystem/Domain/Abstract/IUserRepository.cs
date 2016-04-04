using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using VaultSystem.Models;

namespace VaultSystem.Domain.Abstract
{
    interface IUserRepository
    {
        List<ApplicationUser> GetUsers();
        List<IdentityRole> GetRoles();
        IList<string> GetUserRoles(ApplicationUser user);
        bool AddUser(ApplicationUser user);
        void DeleteUser(ApplicationUser user);
        void AddToRole(ApplicationUser user, IdentityRole role);
        void RemoveFromRole(ApplicationUser user, IdentityRole role);
    }
}
