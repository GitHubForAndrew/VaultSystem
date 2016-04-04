using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultSystem.Models
{
    public class VaultView
    {
        //public ApplicationUser User { get; set; }
        public List<string> UserRoles { get; set; }
        public List<Vault> MyVaults { get; set; }
        public List<Vault> OtherVaults { get; set; }
        public int VaultId { get; set; }
    }

    public class VaultInfo
    {
        public ApplicationUser User { get; set; }
        public List<string> UserRoles { get; set; }
        public int VaultId { get; set; }
    }

    public class VaultItemsView
    {
        public List<VaultItem> VaultItems { get; set; }
        public string VaultAccessLevel { get; set; }
        public int VaultId { get; set; }
    }
}
