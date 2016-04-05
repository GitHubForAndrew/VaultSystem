using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSystem.Models;

namespace VaultSystem.Domain.Abstract
{
    public interface IVaultRepository
    {
        List<Vault> Vaults { get; }
        int CreaetVault(Vault vault, string userId);
        void AddVaultItem(VaultItem item);
        void RemoveVaultItem(VaultItem item);
        void EditVaultAccess(VaultUser vaultUser);
        void RemoveVaultAccess(VaultUser vaultUser);
        Vault Vault(int vaultId);
        List<Vault> GetMyVaults(string userId);
        List<Vault> GetOtherVaults(string userId);
    }
}
