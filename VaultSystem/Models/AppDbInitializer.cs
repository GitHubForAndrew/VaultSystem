using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VaultSystem.Models
{
    class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var globalAdminRole = new IdentityRole("GlobalAdmin");
            var vaultAdminRole = new IdentityRole("VaultAdmin");
            var userRole = new IdentityRole("User");

            roleManager.Create(globalAdminRole);
            roleManager.Create(vaultAdminRole);
            roleManager.Create(userRole);

            var globalAdminUser = new ApplicationUser() { Email = "sa.global@test.ru", UserName = "sa.global@test.ru" };
            var result = userManager.Create(globalAdminUser, "Aq12345");
            if (result.Succeeded)
            {
                userManager.AddToRole(globalAdminUser.Id, globalAdminRole.Name);
            }

            base.Seed(context);
        }
    }
}
