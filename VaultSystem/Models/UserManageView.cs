using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultSystem.Models
{
    public class UserManageView
    {
        private List<ApplicationUser> users;
        private List<IdentityRole> roles;

        public UserManageView(List<ApplicationUser> users, List<IdentityRole> roles)
        {
            this.users = users;
            this.roles = roles;
        }

        public List<ApplicationUser> Users
        {
            get { return users; }
        }

        public List<IdentityRole> Roles
        {
            get { return roles; }
        }

    }
}
