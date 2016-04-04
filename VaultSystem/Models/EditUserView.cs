using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VaultSystem.Models
{
    public class EditUserView
    {
        public ApplicationUser User { get; set; }
        public List<string> UserRoles { get; set; }
        public List<string> AllRoles { get; set; }
        public bool New { get; set; } = false;
    }
}
